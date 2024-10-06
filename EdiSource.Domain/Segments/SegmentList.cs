using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Segments;

/// <summary>
/// Notes a list of segments.
/// In practice, it is a List of ISegment with an IEdi interface for pattern matching purposes. 
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class SegmentList<T> : List<T>, IEdi
    where T : ISegment;