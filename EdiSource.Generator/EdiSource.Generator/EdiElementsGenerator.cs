using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator;

[Generator]
public partial class EdiItemsIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => PredicateOnClassAttributes(ctx, LoopAggregation.LoopGeneratorNames))
            .Where(static m => m is not null)!;

        var compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Right, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<GeneratorItem?> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            return;
        }

        var distinctClasses = classes
            .Distinct()
            .OfType<GeneratorItem>();

        foreach (var (classDeclaration, parent, id) in distinctClasses)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (classSymbol == null) continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();
            
            var ediItems = properties
                .Select(property => new { property, attribute = GetEdiAttribute(property) })
                .Where(t => !string.IsNullOrEmpty(t.attribute))
                .Select(t => (t.property.Name, t.attribute, t.property))
                .ToArray();

            var orderedEdiItems = OrderEdiItems(ediItems);
            
            var ediElementSourceCode = ImplementationSourceCode(className, namespaceName, parent, id);
            context.AddSource($"{className}.EdiElement.g.cs", SourceText.From(ediElementSourceCode, Encoding.UTF8));
            
            var implementationCode = EdiElementSourceCode(className, namespaceName, orderedEdiItems);
            context.AddSource($"{className}.Implementation.g.cs", SourceText.From(implementationCode, Encoding.UTF8));

            var loopConstructorSourceCode = LoopConstructorGenerator.GenerateSourceCode(className, namespaceName, orderedEdiItems);
            context.AddSource($"{className}.Constructor.g.cs", SourceText.From(loopConstructorSourceCode, Encoding.UTF8));
        }
    }
}

public record struct GeneratorItem(ClassDeclarationSyntax ClassDeclarationSyntax, string Parent, string Id);