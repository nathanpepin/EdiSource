using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Structure.GenericTransactionSetData;

/// <summary>
///     Used to serialize any transaction set.
///     This definition will be used if there are no other matching definitions.
/// </summary>
public sealed class GenericTransactionSet : IEdi<FunctionalGroup>, ISegmentIdentifier<GenericTransactionSet>,
    ISegmentIdentifier<GenericST>,
    ITransactionSet<GenericTransactionSet>, ILoopInitialize<FunctionalGroup, GenericTransactionSet>
{
    public GenericST ST { get; set; } = default!;

    public SegmentList<Segment> Segments { get; set; } = [];

    public GenericSE SE { get; set; } = default!;

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

        loop.ST = await SegmentLoopFactory<GenericST, GenericTransactionSet>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (!await ISegmentIdentifier<GenericSE>.MatchesAsync(segmentReader))
            {
                loop.Segments.Add(await segmentReader.ReadAsync());
                continue;
            }

            break;
        }

        loop.SE = await SegmentLoopFactory<GenericSE, GenericTransactionSet>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<GenericTransactionSet>.CreateDefinition();

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