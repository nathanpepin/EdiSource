using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

public static class SegmentFactory<T>
    where T : Segment, ISegmentIdentifier<T>, new()
{
    public static T Create(Queue<ISegment> segments)
    {
        if (!ISegmentIdentifier<T>.Matches(segments.Peek())) throw new ArgumentException($"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {segments}");

        return new T { Elements = segments.Dequeue().Elements };
    }

    public static T Create(ISegment segment)
    {
        if (!ISegmentIdentifier<T>.Matches(segment)) throw new ArgumentException($"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {segment}");

        return new T { Elements = segment.Elements };
    }

    public static T? CreateIfMatches(Queue<ISegment> segments)
    {
        return !ISegmentIdentifier<T>.Matches(segments.Peek())
            ? null
            : new T { Elements = segments.Dequeue().Elements };
    }

    public static T? CreateIfMatches(ISegment segment)
    {
        return ISegmentIdentifier<T>.Matches(segment) ? null : new T { Elements = segment.Elements };
    }
}