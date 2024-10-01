using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class Loop2000_INS : Segment, ISegment<Loop2000>, ISegmentIdentifier<Loop2000_INS>
{
    public Loop2000? Parent { get; }
    
    public static (string Primary, string? Secondary) EdiId => ("INS", null);
}