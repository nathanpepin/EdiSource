using System.Text;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public static class LoopExtensions
{
    // public static IEnumerable<T> FindChildLoops<T>(this ILoop loop) where T : ILoop
    // {
    //     var foundLocation = false;
    //
    //     foreach (var childLoop in loop.YieldChildLoops())
    //     {
    //         if (childLoop is T tChildLoop)
    //         {
    //             yield return tChildLoop;
    //             foundLocation = true;
    //         }
    //
    //         if (foundLocation) continue;
    //
    //         foreach (var innerChildLoop in FindChildLoops<T>(childLoop))
    //         {
    //             yield return innerChildLoop;
    //         }
    //     }
    // }
    //
    // public static StringBuilder WriteToStringBuilder<T>(this T loop, StringBuilder? stringBuilder = null,
    //     Separators? separators = null, bool includeNewLine = true)
    //     where T : ILoop
    // {
    //     separators ??= Separators.DefaultSeparators;
    //     stringBuilder ??= new StringBuilder();
    //
    //     foreach (var segment in loop.YieldChildSegments())
    //     {
    //         segment.WriteToStringBuilder(stringBuilder, separators.Value);
    //
    //         if (includeNewLine)
    //             stringBuilder.AppendLine();
    //     }
    //
    //     return stringBuilder;
    // }
}

public sealed class SegmentList<T> : List<T>, IEdi
where T: ISegment;
    
public sealed class LoopList<T> : List<T>, IEdi
 where T: ILoop;