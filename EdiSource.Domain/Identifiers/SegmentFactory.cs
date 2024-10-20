using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Used for creating segments dynamically
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TLoop"></typeparam>
public static class SegmentLoopFactory<T, TLoop>
    where T : Segment, ISegment<TLoop>, ISegmentIdentifier<T>, new()
    where TLoop : class, ILoop
{
    /// <summary>
    ///     Creates a segment if it matches the criteria
    /// </summary>
    /// <param name="segments"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static T Create(Queue<ISegment> segments, TLoop? parent = null)
    {
        if (!ISegmentIdentifier<T>.Matches(segments.Peek()))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.ToString()}) but received segment: {segments}");

        var segment = segments.Dequeue();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }

    /// <summary>
    ///     Creates a segment is it matches the criteria
    /// </summary>
    /// <param name="segmentReader"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static async ValueTask<T> CreateAsync(ChannelReader<ISegment> segmentReader, TLoop? parent = null)
    {
        await segmentReader.WaitToReadAsync();

        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
            throw new ArgumentException(
                $"Expected ids of ({T.EdiId.ToString()}) but received segment: {await segmentReader.ReadAsync()}");

        var segment = await segmentReader.ReadAsync();
        return new T { Elements = segment.Elements, Parent = parent, Separators = segment.Separators };
    }
}