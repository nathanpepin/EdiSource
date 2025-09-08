namespace EdiSource.Generator.ValidationGen;

[Generator(LanguageNames.CSharp)]
public class ValidationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ValidationContext> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    static (s, _) => IsSyntaxTargetForGeneration(s),
                    static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ValidationContext>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static ValidationContext? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var target = classDeclarationSyntax
            .AttributeLists
            .SelectMany(attributeList => attributeList.Attributes,
                (_, attribute) => context.SemanticModel.GetTypeInfo(attribute))
            .Select(attributeSymbol => new
            {
                TypeSymbol = attributeSymbol, Definition = attributeSymbol.Type?.OriginalDefinition.ToDisplayString()
            })
            .FirstOrDefault(x =>
                x.Definition is "EdiSource.Domain.SourceGeneration.LoopGeneratorAttribute<TParent, TSelf, TId>");

        if (target is not null)
            return new ValidationContext
            {
                ClassDeclarationSyntax = classDeclarationSyntax,
                SubType = GetSegmentGeneratorSubType(context)
            };

        return null;
    }

    private static bool IsTargetAttribute(string attributeName)
    {
        return attributeName switch
        {
            "ElementLength" or "ElementLengthAttribute" or
                "Empty" or "EmptyAttribute" or
                "IsOneOfValues" or "IsOneOfValuesAttribute" or
                "NotEmpty" or "NotEmptyAttribute" or
                "NotOneOfValues" or "NotOneOfValuesAttribute" or
                "RequiredDataElements" or "RequiredDataElementsAttribute" or
                "RequireElement" or "RequiredElementAttribute" or
                "BeDate" or "SegmentElementLengthAttribute" or
                "BeDateTime" or "BeDateTimeAttribute" or
                "BeTime" or "BeTimeAttribute" or
                "BeInt" or "BeIntAttribute" or
                "BeDecimal" or "BeDecimalAttribute" or
                "CompositeElementLength" or "CompositeElementLengthAttribute" => true,
            _ => false
        };
    }

    private static void Execute(Compilation compilation, ImmutableArray<ValidationContext> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty) return;

        foreach (var it in classes)
        {
            var (classDeclaration, subType) = (it.ClassDeclarationSyntax, it.SubType);

            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

            if (classSymbol == null) continue;

            CodeWriter cw = new();

            var sgv =
                compilation.GetTypeByMetadataName("EdiSource.Domain.Validation.Data.ISourceGeneratorValidatable");
            if (sgv == null)
                continue;

            using (cw.StartNamespace(classSymbol.ContainingNamespace.ToDisplayString()))
            {
                foreach (var @using in Usings)
                    cw.AddUsing(@using);

                using (cw.StartClass(classDeclaration.Identifier.Text, ["ISourceGeneratorValidatable"]))
                {
                    cw.AppendLine(subType == null
                        ? "public List<IIndirectValidatable> SourceGenValidations => ["
                        : "new public List<IIndirectValidatable> SourceGenValidations => [");

                    cw.IncreaseIndent();
                    if (subType != null) cw.AppendLine("..base.SourceGenValidations, ");

                    ProcessAttributes(classDeclaration, cw, semanticModel);
                    cw.DecreaseIndent();
                    cw.AppendLine("];");
                }
            }

            context.AddSource($"{classSymbol.Name}.Validation.g.cs", SourceText.From(cw.ToString(), Encoding.UTF8));
        }
    }

    private static void ProcessAttributes(ClassDeclarationSyntax classDeclarationSyntax, CodeWriter cw,
        SemanticModel model)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            var attributeName = attribute.Name.ToString();

            if (!IsTargetAttribute(attributeName)) continue;

            cw.Append("new ");
            cw.Append(attributeName);
            if (!attributeName.EndsWith("Attribute")) cw.Append("Attribute");

            cw.Append("(");
            cw.Append(attribute.ArgumentList?.Arguments.ToString() ?? string.Empty);
            cw.AppendLine("),");
        }
    }

    private sealed class ValidationContext
    {
        public required ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
        public required TypeSyntax? SubType { get; set; }
    }
}