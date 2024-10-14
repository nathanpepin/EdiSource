using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;

namespace EdiSource.Domain.Standard.Loops;

public sealed class FunctionalGroup : ILoop<InterchangeEnvelope>, ISegmentIdentifier<FunctionalGroup>,
    ISegmentIdentifier<GS>, ILoopInitialize<InterchangeEnvelope, FunctionalGroup>
{
    public GS GS { get; set; } = default!;

    public LoopList<ILoop> TransactionSets { get; } = [];

    public GE GE { get; set; } = default!;

    public static Task<FunctionalGroup> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
    {
        if (parent is null)
        {
            return InitializeAsync(segmentReader, null);
        }

        if (parent is not InterchangeEnvelope typedParent)
        {
            throw new ArgumentException($"Parent must be of type {nameof(InterchangeEnvelope)}");
        }

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<FunctionalGroup> InitializeAsync(ChannelReader<ISegment> segmentReader,
        InterchangeEnvelope? parent)
    {
        var loop = new FunctionalGroup
        {
            Parent = parent
        };

        loop.GS = await SegmentLoopFactory<GS, FunctionalGroup>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (!segmentReader.TryPeek(out var segment)) break;

            if (await CreateTransactionSet(segmentReader, segment, loop)) continue;

            break;
        }

        loop.GE = await SegmentLoopFactory<GE, FunctionalGroup>.CreateAsync(segmentReader, loop);

        return loop;
    }

    private static async Task<bool> CreateTransactionSet(ChannelReader<ISegment> segmentReader, ISegment segment,
        FunctionalGroup loop)
    {
        foreach (var ts in InterchangeEnvelope.TransactionSetDefinitions)
        {
            var values = (segment.GetDataElement(0), segment.GetDataElementOrNull(1));
            var reader = ts(values);
            if (reader is null) continue;

            loop.TransactionSets.Add(await reader(segmentReader, loop));
            return true;
        }

        return false;
    }

    public InterchangeEnvelope? Parent { get; set; }
    ILoop? ILoop.Parent => Parent;
    public List<IEdi?> EdiItems => [GS, TransactionSets, GE];
    public static (string Primary, string? Secondary) EdiId => GS.EdiId;
}