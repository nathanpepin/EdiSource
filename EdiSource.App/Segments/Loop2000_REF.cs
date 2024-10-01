using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class Loop2000_REF : Segment, ISegment<Loop2000>, ISegmentIdentifier<Loop2000_REF>
{
    public Loop2000? Parent { get; }

    public static (string Primary, string? Secondary) EdiId => ("REF", null);
}