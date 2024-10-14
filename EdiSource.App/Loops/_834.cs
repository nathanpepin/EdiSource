using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<FunctionalGroup, _834, TS_ST>]
public sealed partial class _834 : ITransactionSet<_834, TS_ST>
{
    private TransactionSetDefinition<_834> _definition;
    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; }

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; }
    
    ISegment ITransactionSet.ST => ST;

    ISegment ITransactionSet.SE => SE;

    private static async Task<ILoop> EnvelopeInitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
    {
        var result = await InitializeAsync(segmentReader, parent);
        return result;
    }

    public static TransactionSetDefinition Definition { get; } = id =>
    {
        if (EdiId.Primary != id.Item1 || (EdiId.Secondary is not null && EdiId.Secondary != id.Item2))
        {
            return null;
        }

        return EnvelopeInitializeAsync;
    };
}