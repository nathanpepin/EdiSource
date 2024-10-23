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

        var t = new T { Elements = segment.Elements, Separators = segment.Separators };
        if (t is IEdi<TLoop> e) 
            e.Parent = parent;
        return t;
    }
}