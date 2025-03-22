namespace EdiSource.Domain.Standard.Segments;

public sealed class GenericSegment<T> : Segment, IEdi<T> where T : IEdi
{
    public T? Parent { get; set; }
}