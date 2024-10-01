using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator]
public class Loop2000 : ILoop<TransactionSet>, ISegmentIdentifier<Loop2000_INS>, ISegmentIdentifier<Loop2000>
{
    public Loop2000(Queue<ISegment> segments)
    {
        //Header
        INS = SegmentFactory<Loop2000_INS>.Create(segments);
    }

    [SegmentHeader] public Loop2000_INS INS { get; set; }

    ILoop? ILoop.Parent => Parent;

    public TransactionSet? Parent { get; }

    public IEnumerable<ISegment> YieldChildSegments()
    {
        yield return INS;
    }

    public IEnumerable<ILoop> YieldChildLoops()
    {
        return [];
    }

    public static (string Primary, string? Secondary) EdiId => Loop2000_INS.EdiId;
}