using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class ST : Segment, ISegment<GenericTransactionSet>, ISegmentIdentifier<ST>
{
    public GenericTransactionSet? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("ST", null);
}