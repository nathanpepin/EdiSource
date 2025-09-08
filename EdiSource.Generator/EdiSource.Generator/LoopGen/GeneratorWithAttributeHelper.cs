namespace EdiSource.Generator.LoopGen;

public static class GeneratorWithAttributeHelper<T> where T : struct
{
    public static void Initialize(
        IncrementalGeneratorInitializationContext context,
        string attributeMetaDataName,
        Func<SyntaxNode, CancellationToken, bool> predicate,
        Func<GeneratorAttributeSyntaxContext, CancellationToken, T?> transform,
        Action<SourceProductionContext, ImmutableArray<T>> createOutput
    )
    {
        var source = context.SyntaxProvider.ForAttributeWithMetadataName(
                attributeMetaDataName,
                predicate,
                transform)
            .Where(x => x.HasValue)
            .Select((t, _) => t!.Value);

        context.RegisterSourceOutput(source.Collect(), createOutput);
    }

    public static bool Predicate(SyntaxNode node, CancellationToken ct, Func<SyntaxNode, CancellationToken, bool> fun)
    {
        return fun(node, ct);
    }

    public static T Transform(GeneratorAttributeSyntaxContext context, CancellationToken ct,
        Func<GeneratorAttributeSyntaxContext, CancellationToken, T> fun)
    {
        return fun(context, ct);
    }

    public static void CreateOutput(SourceProductionContext context, ImmutableArray<T> source,
        Action<SourceProductionContext, ImmutableArray<T>> act)
    {
        act(context, source);
    }
}