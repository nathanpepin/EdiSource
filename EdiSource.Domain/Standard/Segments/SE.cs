using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class SE : Segment, ISegment<GenericTransactionSet>, ISegmentIdentifier<SE>
{
    
    public GenericTransactionSet? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("SE", null);
}