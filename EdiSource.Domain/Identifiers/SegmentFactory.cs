using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

public static class SegmentLoopFactory<T, TLoop>
    where T : Segment, ISegment<TLoop>, ISegmentIdentifier<T>, new()
    where TLoop : class, ILoop
{
    public static T Create(Queue<ISegment> segments, TLoop? parent = null)
    {
        if (!ISegmentIdentifier<T>.Matches(segments.Peek()))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {segments}");

        return new T { Elements = segments.Dequeue().Elements, Parent = parent };
    }

    public static T Create(ISegment segment, TLoop? parent = null)
    {
        if (!ISegmentIdentifier<T>.Matches(segment))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {segment}");

        return new T { Elements = segment.Elements, Parent = parent };
    }

    public static T? CreateIfMatches(Queue<ISegment> segments, TLoop? parent = null)
    {
        return !ISegmentIdentifier<T>.Matches(segments.Peek())
            ? null
            : new T { Elements = segments.Dequeue().Elements, Parent = parent };
    }

    public static T? CreateIfMatches(ISegment segment, TLoop? parent = null)
    {
        return ISegmentIdentifier<T>.Matches(segment) ? null : new T { Elements = segment.Elements, Parent = parent };
    }

    public static async Task<T> CreateAsync(ChannelReader<ISegment> segmentReader, TLoop? parent = null)
    {
        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {await segmentReader.ReadAsync()}");

        var segment = await segmentReader.ReadAsync();
        return new T { Elements = segment.Elements, Parent = parent };
    }

    public static async Task<T?> CreateIfMatchesAsync(ChannelReader<ISegment> segmentReader, TLoop? parent = null)
    {
        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
            return null;

        var segment = await segmentReader.ReadAsync();
        return new T { Elements = segment.Elements, Parent = parent };
    }
}