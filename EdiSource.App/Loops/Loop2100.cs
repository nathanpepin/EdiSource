using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator]
public partial class Loop2100 : ILoop<TransactionSet>, ISegmentIdentifier<Loop2100_NM1>, ISegmentIdentifier<Loop2100>
{
    public Loop2100(Queue<ISegment> segments, TransactionSet? parent = null)
    {
        Parent = parent;
        
        //Header
        NM1 = SegmentLoopFactory<Loop2100_NM1, Loop2100>.Create(segments, this);
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