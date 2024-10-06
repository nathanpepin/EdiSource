using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    public static IEnumerable<ISegment> YieldChildSegments<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<ISegment> items = [];
        SegmentAction(it, x => items.Add(x), recursive);
        return items;
    }
}