using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

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

        var segment = segments.Dequeue();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    public static T Create(ISegment segment, TLoop? parent = null)
    {
        if (!ISegmentIdentifier<T>.Matches(segment))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {segment}");

        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    public static T? CreateIfMatches(Queue<ISegment> segments, TLoop? parent = null)
    {
        if (!ISegmentIdentifier<T>.Matches(segments.Peek())) return null;

        var segment = segments.Dequeue();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    public static T? CreateIfMatches(ISegment segment, TLoop? parent = null)
    {
        return !ISegmentIdentifier<T>.Matches(segment)
            ? null
            : new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    public static async Task<T> CreateAsync(ChannelReader<ISegment> segmentReader, TLoop? parent = null)
    {
        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.Primary}, {T.EdiId.Secondary}) but received segment: {await segmentReader.ReadAsync()}");

        var segment = await segmentReader.ReadAsync();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    public static async Task<T?> CreateIfMatchesAsync(ChannelReader<ISegment> segmentReader, TLoop? parent = null)
    {
        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
            return null;

        var segment = await segmentReader.ReadAsync();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }
}