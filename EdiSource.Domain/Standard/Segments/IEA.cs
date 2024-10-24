using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops.ISA;

namespace EdiSource.Domain.Standard.Segments;

public sealed class IEA : Segment, IEdi<InterchangeEnvelope>, ISegmentIdentifier<IEA>
{
    public InterchangeEnvelope? Parent { get; set; }
    public static EdiId EdiId { get; } = new("IEA");
}