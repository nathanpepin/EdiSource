using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<FunctionalGroup, _834, TS_ST>()]
public sealed partial class _834 : ITransactionSet<_834, TS_ST, TS_SE>
{
    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP? DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; } = default!;

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];
    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    public static TransactionSetDefinition Definition { get; } = id =>
    {
        if (!EdiId.MatchesSegment(id)) return null;

        return (segmentReader, parent) => InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
    };

    ST ITransactionSet.ST => ST;
    SE ITransactionSet.SE => SE;

    [SegmentFooter] public TS_SE SE { get; set; } = default!;
}