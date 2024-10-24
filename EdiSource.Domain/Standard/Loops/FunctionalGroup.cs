using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.Standard.Segments;

namespace EdiSource.Domain.Standard.Loops;

public sealed class FunctionalGroup : IEdi<InterchangeEnvelope>, ISegmentIdentifier<FunctionalGroup>,
    ISegmentIdentifier<GS>, ILoopInitialize<InterchangeEnvelope, FunctionalGroup>
{
    private static readonly EdiId _st = new("ST");

    public GS GS { get; set; } = default!;

    public LoopList<ILoop> TransactionSets { get; } = [];

    public GE GE { get; set; } = default!;

    public InterchangeEnvelope? Parent { get; set; }
    public List<IEdi?> EdiItems => [GS, TransactionSets, GE];

    public static Task<FunctionalGroup> InitializeAsync(ChannelReader<Segment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not InterchangeEnvelope typedParent)
            throw new ArgumentException($"Parent must be of type {nameof(InterchangeEnvelope)}");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<FunctionalGroup> InitializeAsync(ChannelReader<Segment> segmentReader,
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

            if (!_st.MatchesSegment(segment)) break;

            if (await CreateTransactionSet(segmentReader, segment, loop)) continue;

            break;
        }

        loop.GE = await SegmentLoopFactory<GE, FunctionalGroup>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public static EdiId EdiId => GS.EdiId;

    private static async Task<bool> CreateTransactionSet(ChannelReader<Segment> segmentReader, Segment segment,
        FunctionalGroup loop)
    {
        foreach (var ts in InterchangeEnvelope.TransactionSetDefinitions)
        {
            var reader = ts(segment);
            if (reader is null) continue;

            loop.TransactionSets.Add(await reader(segmentReader, loop));
            return true;
        }

        var generic = GTS.Definition(segment);
        if (generic is null) return false;

        loop.TransactionSets.Add(await generic(segmentReader, loop));
        return true;
    }
}