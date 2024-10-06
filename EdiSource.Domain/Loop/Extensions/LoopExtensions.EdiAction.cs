using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    public static void EdiAction<T>(this T it,
        Action<ISegment>? segmentAction = null,
        Action<SegmentList<ISegment>>? segmentListAction = null,
        Action<ILoop>? loopAction = null,
        Action<LoopList<ILoop>>? loopListAction = null)
        where T : ILoop
    {
        foreach (var ediItem in it.EdiItems)
            switch (ediItem)
            {
                case null: continue;
                case ISegment segment:
                    segmentAction?.Invoke(segment);
                    continue;
                case IEnumerable<ISegment> segmentList:
                    segmentListAction?.Invoke([..segmentList]);
                    continue;
                case ILoop loop:
                    loopAction?.Invoke(loop);
                    continue;
                case IEnumerable<ILoop> loopList:
                    loopListAction?.Invoke([..loopList]);
                    continue;
            }
    }
}