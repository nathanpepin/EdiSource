using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator]
public class Loop2100 : ILoop<TransactionSet>, ISegmentIdentifier<Loop2100_NM1>, ISegmentIdentifier<Loop2100>
{
    public Loop2100(Queue<ISegment> segments)
    {
        //Header
        NM1 = SegmentFactory<Loop2100_NM1>.Create(segments);
    }

    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; }

    ILoop? ILoop.Parent => Parent;

    public TransactionSet? Parent { get; }

    public IEnumerable<ISegment> YieldChildSegments()
    {
        yield return NM1;
    }

    public IEnumerable<ILoop> YieldChildLoops()
    {
        return [];
    }

    public static (string Primary, string? Secondary) EdiId => Loop2100_NM1.EdiId;
}