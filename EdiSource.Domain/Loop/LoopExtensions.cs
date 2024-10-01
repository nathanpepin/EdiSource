using System.Text;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public static class LoopExtensions
{
    public static IEnumerable<ISegment> RecursiveYieldSegments(this ILoop? loop)
    {
        if (loop is null) yield break;

        foreach (var segment in loop.YieldChildSegments())
        {
            yield return segment;
        }

        foreach (var childLoop in loop.YieldChildLoops())
        {
            foreach (var subSegment in childLoop.RecursiveYieldSegments())
            {
                yield return subSegment;
            }
        }
    }

    public static IEnumerable<ILoop> RecursiveYieldLoops(this ILoop? loop)
    {
        if (loop is null) yield break;

        foreach (var childLoop in loop.YieldChildLoops())
        {
            yield return childLoop;

            foreach (var subLoop in childLoop.RecursiveYieldLoops())
            {
                yield return subLoop;
            }
        }
    }

    public static IEnumerable<T> FindChildLoops<T>(this ILoop loop) where T : ILoop
    {
        var foundLocation = false;

        foreach (var childLoop in loop.YieldChildLoops())
        {
            if (childLoop is T tChildLoop)
            {
                yield return tChildLoop;
                foundLocation = true;
            }

            if (foundLocation) continue;

            foreach (var innerChildLoop in FindChildLoops<T>(childLoop))
            {
                yield return innerChildLoop;
            }
        }
    }

    public static StringBuilder WriteToStringBuilder<T>(this T loop, StringBuilder? stringBuilder = null, Separators? separators = null, bool includeNewLine = true)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;
        stringBuilder ??= new StringBuilder();

        foreach (var segment in loop.RecursiveYieldSegments())
        {
            segment.WriteToStringBuilder(stringBuilder, separators.Value);

            if (includeNewLine)
                stringBuilder.AppendLine();
        }

        return stringBuilder;
    }
}