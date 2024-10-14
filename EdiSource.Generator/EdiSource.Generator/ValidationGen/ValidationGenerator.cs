using System.Collections.Immutable;
using System.Text;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator.ValidationGen;

[Generator(LanguageNames.CSharp)]
public class ValidationGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor GenericError = new("HS1000",
        "Failed to generate validation code",
        "Couldn't autogenerate validation code for class {0} due to reason {1}",
        "Honlsoft.DependencyInjection.SourceGenerators",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    static (s, _) => IsSyntaxTargetForGeneration(s),
                    static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };
    }

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            var attributeName = attribute.Name.ToString();
            if (IsTargetAttribute(attributeName)) return classDeclarationSyntax;
        }

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
                "BeDecimal" or "BeDecimalAttribute" => true,
            _ => false
        };
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty) return;

        foreach (var classDeclaration in classes)
            try
            {
                var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

                if (classSymbol == null) continue;

                CodeWriter cw = new();

                foreach (var attribute in classSymbol.GetAttributes())
                {
                    var attributeType = attribute.AttributeClass;
                    if (attributeType == null)
                        continue;

                    // Replace IYourInterface with the actual interface you're checking for
                    var interfaceType =
                        compilation.GetTypeByMetadataName("EdiSource.Domain.Validation.Data.IIndirectValidatable");
                    if (interfaceType == null)
                        continue;

                    if (attributeType.AllInterfaces.Contains(interfaceType)) cw.AppendLine($"// {attributeType.Name}");
                    // The attribute implements the interface
                    // You can generate code or perform other actions here
                }

                using (cw.StartNamespace(classSymbol.ContainingNamespace.ToDisplayString()))
                {
                    foreach (var @using in Usings)
                        cw.AddUsing(@using);

                    using (cw.StartClass(classDeclaration.Identifier.Text, ["ISourceGeneratorValidatable"]))
                    {
                        cw.AppendLine("public List<IIndirectValidatable> SourceGenValidations => [");
                        cw.IncreaseIndent();
                        ProcessAttributes(classDeclaration, cw, semanticModel);
                        cw.DecreaseIndent();
                        cw.AppendLine("];");
                    }
                }

                context.AddSource($"{classSymbol.Name}.Validation.g.cs", SourceText.From(cw.ToString(), Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(GenericError, null, ex, classDeclaration.Identifier.Text));
            }
    }

    private static void ProcessAttributes(ClassDeclarationSyntax classDeclarationSyntax, CodeWriter cw,
        SemanticModel model)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            var attributeName = attribute.Name.ToString();

            if (attributeName is not ("ElementLength" or "ElementLengthAttribute" or
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
                "BeDecimal" or "BeDecimalAttribute")) continue;

            var attributeSymbol = model.GetSymbolInfo(attribute);

            cw.Append("new ");
            cw.Append(attributeName);
            if (!attributeName.EndsWith("Attribute")) cw.Append("Attribute");

            cw.Append("(");
            cw.Append(attribute.ArgumentList?.Arguments.ToString());
            cw.AppendLine("),");
        }
    }
}