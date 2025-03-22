using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GenericSegment<T> : Segment, IEdi<T> where T : IEdi
{
    public T? Parent { get; set; }
}