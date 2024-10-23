using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Structure;

/// <summary>
///     A non-typed edi structure with only segments and their seperator definition.
///     Can be used in cases where there is no edi implementation.
///     <br /><br />
///     Implements ILoop, so can be used in serialzation.
/// </summary>
/// <param name="segments"></param>
/// <param name="separators"></param>
public sealed class BasicEdi(IEnumerable<Segment> segments, Separators separators)
    : ILoop
{
    public List<Segment> Segments { get; } = segments.ToList();
    public Separators Separators { get; } = separators;

    public List<IEdi?> EdiItems => [..segments.Cast<IEdi?>()];

    public void Deconstruct(out List<Segment> outSegments, out Separators outSeparators)
    {
        outSegments = Segments;
        outSeparators = Separators;
    }
}