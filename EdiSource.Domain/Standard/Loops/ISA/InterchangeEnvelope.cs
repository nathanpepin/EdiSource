using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Loops;

public sealed class InterchangeEnvelope : ILoop<InterchangeEnvelope>, ISegmentIdentifier<InterchangeEnvelope>,
    ISegmentIdentifier<ISA>, ILoopInitialize<InterchangeEnvelope, InterchangeEnvelope>
{
    public static List<TransactionSetDefinition> TransactionSetDefinitions = [];

    public ISA ISA { get; set; } = default!;

    public LoopList<FunctionalGroup> FunctionalGroups { get; } = [];

    public ISegment IEA { get; set; } = default!;

    public InterchangeEnvelope? Parent => null;

    ILoop? ILoop.Parent => Parent;
    public List<IEdi?> EdiItems => [ISA, FunctionalGroups, IEA];

    public static Task<InterchangeEnvelope> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not InterchangeEnvelope typedParent)
            throw new ArgumentException($"Parent must be of type {nameof(InterchangeEnvelope)}");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<InterchangeEnvelope> InitializeAsync(ChannelReader<ISegment> segmentReader,
        InterchangeEnvelope? parent)
    {
        var loop = new InterchangeEnvelope();

        loop.ISA = await SegmentLoopFactory<ISA, InterchangeEnvelope>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (await ISegmentIdentifier<FunctionalGroup>.MatchesAsync(segmentReader))
            {
                loop.FunctionalGroups.Add(await FunctionalGroup.InitializeAsync(segmentReader, loop));
                continue;
            }

            break;
        }

        loop.IEA = await SegmentLoopFactory<IEA, InterchangeEnvelope>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public static EdiId EdiId => ISA.EdiId;
}