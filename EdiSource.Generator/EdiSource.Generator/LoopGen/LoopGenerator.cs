using System.Collections.Immutable;
using System.Text;
using EdiSource.Generator.LoopGen.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator.LoopGen;

[Generator]
public partial class LoopGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (s, _) => IsSyntaxTargetForGeneration(s, LoopAggregation.LoopGeneratorNames),
                static (ctx, _) => PredicateOnClassAttributes(ctx, LoopAggregation.LoopGeneratorNames));

        var compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Right, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<LoopMeta> classes,
        SourceProductionContext context)
    {
        foreach (var generatorItem in classes)
        {
            var (classDeclaration, parent, self, id, isTransactionSet) = generatorItem;

            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();

            var classUsings = GetUsingStatements(classDeclaration)
                .Select(x => x.Name?.ToString())
                .OfType<string>()
                .ToImmutableHashSet();

            HashSet<string> usings = [..classUsings, ..Usings];

            var ediItems = properties
                .Select(property => new { property, attribute = GetEdiAttribute(property) })
                .Where(t => !string.IsNullOrEmpty(t.attribute))
                .Select(t => (t.property.Name.Replace("?", ""), t.attribute.Replace("?", ""), t.property))
                .ToArray();

            var orderedEdiItems = OrderEdiItems(ediItems);

            var ediElementSourceCode = EdiElementGenerator.Generate(className, namespaceName, usings, orderedEdiItems);
            context.AddSource($"{className}.EdiElement.g.cs", SourceText.From(ediElementSourceCode, Encoding.UTF8));

            var implementationCode = ImplementationGenerator.Generate(className, namespaceName, usings, parent, id);
            context.AddSource($"{className}.Implementation.g.cs", SourceText.From(implementationCode, Encoding.UTF8));

            if (isTransactionSet)
            {
                var transactionSetCode = TransactionSetGenerator.Generate(className, namespaceName, usings, id);
                context.AddSource($"{className}.TransactionSet.g.cs",
                    SourceText.From(transactionSetCode, Encoding.UTF8));
            }

            var channelConstructorSourceCode =
                ChannelConstructorGenerator.Generate(className, namespaceName, usings, orderedEdiItems, parent);
            context.AddSource($"{className}.ChannelConstructor.g.cs",
                SourceText.From(channelConstructorSourceCode, Encoding.UTF8));
        }
    }
}