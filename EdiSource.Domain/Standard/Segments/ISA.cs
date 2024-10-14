using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class ISA : Segment, ISegment<InterchangeEnvelope>, ISegmentIdentifier<ISA>
{
    public new InterchangeEnvelope? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("ISA", null);
}