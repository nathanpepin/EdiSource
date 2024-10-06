using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    /// Executes the specified actions on different types of EDI items within the loop.
    /// </summary>
    /// <typeparam name="T">The type of the loop, which must implement <see cref="ILoop"/>.</typeparam>
    /// <param name="it">The instance of the loop on which the actions are to be executed.</param>
    /// <param name="segmentAction">An action to be performed on individual segments, or null.</param>
    /// <param name="segmentListAction">An action to be performed on lists of segments, or null.</param>
    /// <param name="loopAction">An action to be performed on individual loops, or null.</param>
    /// <param name="loopListAction">An action to be performed on lists of loops, or null.</param>
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