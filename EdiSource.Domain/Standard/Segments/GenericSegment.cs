using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GenericSegment : Segment, IEdi<GenericTransactionSet>
{
    public GenericTransactionSet? Parent { get; set; }
}