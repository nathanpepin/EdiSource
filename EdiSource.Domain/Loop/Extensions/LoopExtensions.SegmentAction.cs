using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
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