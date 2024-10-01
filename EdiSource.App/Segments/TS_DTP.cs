using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_DTP : Segment, ISegment<TransactionSet>, ISegmentIdentifier<TS_DTP>
{
    public TransactionSet? Parent { get; }

    public static (string Primary, string? Secondary) EdiId => ("DTP", null);
}