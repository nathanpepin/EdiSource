using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Structure;

public sealed class BasicEdi(IEnumerable<ISegment> segments, Separators separators)
{
    public IEnumerable<ISegment> Segments { get; } = segments;
    public Separators Separators { get; } = separators;

    public void Deconstruct(out IEnumerable<ISegment> outSegments, out Separators outSeparators)
    {
        outSegments = Segments;
        outSeparators = Separators;
    }
}