using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    ///     Iterates over segments in the loop and applies a given action to each segment.
    /// </summary>
    /// <typeparam name="T">The type of loop, which must implement the ILoop interface.</typeparam>
    /// <param name="it">The loop instance on which to perform the operation.</param>
    /// <param name="action">The action to apply to each segment.</param>
    /// <param name="recursive">Indicates whether to recursively apply the action to child segments. Default is true.</param>
    public static void SegmentAction<T>(this T it, Action<ISegment> action, bool recursive = true)
        where T : ILoop
    {
        EdiAction(it,
            action,
            segmentList =>
            {
                foreach (var segment in segmentList) action(segment);
            },
            loop =>
            {
                if (!recursive) return;
                foreach (var childSegment in loop.YieldChildSegments()) action(childSegment);
            },
            loopList =>
            {
                if (!recursive) return;
                foreach (var childSegment in loopList.SelectMany(x => x.YieldChildSegments())) action(childSegment);
            });
    }
}