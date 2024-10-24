using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Standard.Loops;

public sealed class GTS_ST : ST<GTS>, ISegmentIdentifier<GTS_ST>;

public sealed class GTS_SE : SE<GTS>, ISegmentIdentifier<GTS_SE>;

/// <summary>
///     Used to serialize any transaction set.
///     This definition will be used if there are no other matching definitions.
/// </summary>
public sealed class GTS : ILoop, IEdi<FunctionalGroup>, ISegmentIdentifier<GTS>, ISegmentIdentifier<ST<GTS>>,
    ITransactionSet
{
    public GTS_ST ST { get; set; } = default!;

    public SegmentList<Segment> Segments { get; set; } = [];

    public GTS_SE SE { get; set; } = default!;

    public static Task<GTS> InitializeAsync(ChannelReader<Segment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not FunctionalGroup typedParent)
            throw new ArgumentException("Parent must be of type FunctionalGroup");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<GTS> InitializeAsync(ChannelReader<Segment> segmentReader,
        FunctionalGroup? parent)
    {
        var loop = new GTS
        {
            Parent = parent
        };

        loop.ST = await SegmentLoopFactory<GTS_ST, GTS>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (!await ISegmentIdentifier<SE<GTS>>.MatchesAsync(segmentReader))
            {
                loop.Segments.Add(await segmentReader.ReadAsync());
                continue;
            }

            break;
        }

        loop.SE = await SegmentLoopFactory<GTS_SE, GTS>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public static TransactionSetDefinition Definition { get; } = id =>
    {
        if (EdiId.MatchesSegment(id)) return null;

        return (segmentReader, parent) => InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
    };

    public int GetTransactionSetSegmentCount()
    {
        return this.CountSegments();
    }

    public string GetTransactionSetControlNumber()
    {
        return ST.TransactionSetControlNumber;
    }

    public List<IEdi?> EdiItems => [ST, Segments, SE];

    public static EdiId EdiId { get; } = new("ST");
    public FunctionalGroup? Parent { get; set; }
}