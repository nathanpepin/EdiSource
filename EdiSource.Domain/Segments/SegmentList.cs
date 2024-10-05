using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Segments;

public sealed class SegmentList<T> : List<T>, IEdi
    where T : ISegment;