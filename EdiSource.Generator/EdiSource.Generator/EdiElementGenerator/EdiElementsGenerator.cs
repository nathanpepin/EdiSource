using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator.EdiElementGenerator;

[Generator]
public partial class EdiItemsIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s, LoopAggregation.LoopGeneratorNames),
                transform: static (ctx, _) => PredicateOnClassAttributes(ctx, LoopAggregation.LoopGeneratorNames));

        var compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Right, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<GeneratorItem> classes,
        SourceProductionContext context)
    {
        foreach (var generatorItem in classes)
        {
            var (classDeclaration, parent, self, id) = generatorItem;

            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol) continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();
            
            var usings = GetUsingStatements(classDeclaration)
                .Select(x => x.Name?.ToString())
                .OfType<string>()
                .ToImmutableArray();

            var ediItems = properties
                .Select(property => new { property, attribute = GetEdiAttribute(property) })
                .Where(t => !string.IsNullOrEmpty(t.attribute))
                .Select(t => (t.property.Name, t.attribute, t.property))
                .ToArray();

            var orderedEdiItems = OrderEdiItems(ediItems);

            var ediElementSourceCode = EdiElementGenerator.Generate(className, namespaceName, usings, orderedEdiItems);
            context.AddSource($"{className}.EdiElement.g.cs", SourceText.From(ediElementSourceCode, Encoding.UTF8));

            var implementationCode = ImplementationGenerator.Generate(className, namespaceName, usings, parent, id);
            context.AddSource($"{className}.Implementation.g.cs", SourceText.From(implementationCode, Encoding.UTF8));

            var loopConstructorSourceCode =
                QueueConstructorGenerator.Generate(className, namespaceName, usings, orderedEdiItems, parent);
            context.AddSource($"{className}.QueueConstructor.g.cs",
                SourceText.From(loopConstructorSourceCode, Encoding.UTF8));

            var channelConstructorSourceCode =
                ChannelConstructorGenerator.Generate(className, namespaceName, usings, orderedEdiItems, generatorItem);
            context.AddSource($"{className}.ChannelConstructor.g.cs",
                SourceText.From(channelConstructorSourceCode, Encoding.UTF8));
        }
    }
}