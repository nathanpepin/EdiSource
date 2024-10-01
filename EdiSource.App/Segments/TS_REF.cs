using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_REF : Segment, ISegment<TransactionSet>, ISegmentIdentifier<TS_REF>
{
    public TransactionSet? Parent { get; }
    public static (string Primary, string? Secondary) EdiId => ("REF", null);
}