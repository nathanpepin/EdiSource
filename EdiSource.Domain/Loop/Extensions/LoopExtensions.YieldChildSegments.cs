using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    ///     Yields all child segments of the given loop.
    ///     Optionally performs this action recursively on nested loops.
    /// </summary>
    /// <typeparam name="T">Type of the loop, which must implement ILoop.</typeparam>
    /// <param name="it">The loop instance from which child segments are to be retrieved.</param>
    /// <param name="recursive">If true, the function will include child segments from nested loops recursively.</param>
    /// <returns>An IEnumerable of Segment representing the child segments of the loop.</returns>
    public static IEnumerable<Segment> YieldChildSegments<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<Segment> items = [];
        SegmentAction(it, x => items.Add(x), recursive);
        return items;
    }
}