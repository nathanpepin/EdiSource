using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator.SegmentGenerator;

[Generator]
public partial class SegmentGeneratorIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (s, _) => IsSyntaxTargetForGeneration(s, LoopAggregation.SegmentGeneratorNames),
                static (ctx, _) => PredicateOnClassAttributesClassParent(ctx, LoopAggregation.SegmentGeneratorNames));

        var compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Right, spc));
    }

    private static void Execute(Compilation compilation,
        ImmutableArray<(ClassDeclarationSyntax, string loop, string primaryId, string secondaryId)> classes,
        SourceProductionContext context)
    {
        foreach (var (classDeclaration, parent, primaryId, secondaryId) in classes)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();

            var classUsings = GetUsingStatements(classDeclaration)
                .Select(x => x.Name?.ToString())
                .OfType<string>()
                .ToImmutableArray();

            var implementationCode = Generate(className, namespaceName, [..classUsings, ..Usings], parent, primaryId,
                secondaryId);
            context.AddSource($"{className}.Implementation.g.cs", SourceText.From(implementationCode, Encoding.UTF8));
        }
    }
}