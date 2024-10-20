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
    ITransactionSet<GenericTransactionSet, ST, SE>
{
    public ST ST { get; set; } = default!;
    public SegmentList<ISegment> Segments { get; set; } = [];
    public SE SE { get; set; } = default!;
    public static (string Primary, string? Secondary) EdiId { get; } = ("ST", null);

    public static Task<GenericTransactionSet> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not FunctionalGroup typedParent)
            throw new ArgumentException("Parent must be of type FunctionalGroup");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<GenericTransactionSet> InitializeAsync(ChannelReader<ISegment> segmentReader,
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

    ILoop? ILoop.Parent => Parent;
    public FunctionalGroup? Parent { get; set; }
    public List<IEdi?> EdiItems => [ST, Segments, SE];

    public static TransactionSetDefinition Definition { get; } = id =>
    {
        if (EdiId.Primary != id.Item1 || (EdiId.Secondary is not null && EdiId.Secondary != id.Item2)) return null;

        return (segmentReader, parent) => InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
    };
}