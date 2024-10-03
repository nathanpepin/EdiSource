using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator;

[Generator]
public class EdiItemsIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        return classDeclarationSyntax;

        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeSymbol)
                {
                    INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    string fullName = attributeContainingTypeSymbol.ToDisplayString();

                    if (fullName == "LoopGeneratorAttribute")
                    {
                        return classDeclarationSyntax;
                    }
                }
            }
        }

        return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            return;
        }

        var distinctClasses = classes.Distinct();

        foreach (var classDeclaration in distinctClasses)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (classSymbol == null) continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();

            var ediItems = new List<(string Name, string Attribute, IPropertySymbol PropertySymbol)>();

            foreach (var property in properties)
            {
                var attribute = GetEdiAttribute(property);
                if (!string.IsNullOrEmpty(attribute))
                {
                    ediItems.Add((property.Name, attribute, property));
                }
            }

            var orderedEdiItems = OrderEdiItems(ediItems);
            var sourceCode = GenerateSourceCode(className, namespaceName, orderedEdiItems);

            context.AddSource($"{className}.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
        }
    }

    private static string GenerateSourceCode(string className, string namespaceName,
        List<(string Name, string Attribute, IPropertySymbol PropertySymbol)> orderedEdiItems)
    {
        var cw = new CodeWriter();

        cw.AddUsing("System.Collections.Generic");
        cw.AddUsing("EdiSource.Domain.Identifiers");
        cw.AppendLine();
        using (var ns = cw.StartNamespace(namespaceName))
        {
            using (var cl = cw.StartClass(className))
            {
                var names = orderedEdiItems.Select(x => x.Name).ToArray();
                cw.AddCalcProperty("EdiItems", "List<IEdi?>", $$"""new List<IEdi?> { {{string.Join(", ", names)}} }""");
            }
        }

        return cw.ToString();
    }
}