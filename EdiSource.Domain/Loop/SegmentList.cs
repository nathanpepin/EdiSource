using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop;

public sealed class SegmentList<T> : List<T>, IEdi
    where T : ISegment;