using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Standard.Loops;

/// <summary>
///     Used to serialize any transaction set.
///     This definition will be used if there are no other matching definitions.
/// </summary>
public sealed class GenericTransactionSet :
    ISegmentIdentifier<GenericTransactionSet>,
    ITransactionSet<GenericTransactionSet, Generic_ST, Generic_SE>
{
    public SegmentList<Segment> Segments { get; set; } = [];
    public static EdiId EdiId => Generic_ST.EdiId;
    public Generic_ST ST { get; set; } = default!;
    SE ITransactionSet.SE => SE;
    ST ITransactionSet.ST => ST;
    public Generic_SE SE { get; set; } = default!;

    public static Task<GenericTransactionSet> InitializeAsync(ChannelReader<Segment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not FunctionalGroup typedParent)
            throw new ArgumentException("Parent must be of type FunctionalGroup");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<GenericTransactionSet> InitializeAsync(ChannelReader<Segment> segmentReader,
        FunctionalGroup? parent)
    {
        var loop = new GenericTransactionSet
        {
            Parent = parent
        };

        loop.ST = await SegmentLoopFactory<Generic_ST, GenericTransactionSet>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (!await ISegmentIdentifier<SE>.MatchesAsync(segmentReader))
            {
                loop.Segments.Add(await segmentReader.ReadAsync());
                continue;
            }

            break;
        }

        loop.SE = await SegmentLoopFactory<Generic_SE, GenericTransactionSet>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public FunctionalGroup? Parent { get; set; }
    public List<IEdi?> EdiItems => [ST, Segments, SE];

    public static TransactionSetDefinition Definition { get; } = id =>
    {
        if (EdiId.MatchesSegment(id)) return null;

        return (segmentReader, parent) => InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
    };
}