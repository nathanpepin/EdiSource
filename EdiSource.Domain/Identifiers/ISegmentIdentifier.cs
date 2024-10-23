using System.Threading.Channels;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Used to identify a segment
/// </summary>
public interface ISegmentIdentifier
{
    static abstract EdiId EdiId { get; }
}

/// <summary>
///     Matches segments to their identifiers
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISegmentIdentifier<T> : ISegmentIdentifier
    where T : ISegmentIdentifier<T>
{
    /// <summary>
    ///     Matches a segment using the segment identifiers
    /// </summary>
    /// <param name="segment"></param>
    /// <returns></returns>
    public static bool Matches(Segment segment)
    {
        return T.EdiId.MatchesSegment(segment);
    }

    /// <summary>
    ///     Matches the first item from a Channel using the segment identifiers
    /// </summary>
    /// <param name="segmentReader"></param>
    /// <returns></returns>
    public static async ValueTask<bool> MatchesAsync(ChannelReader<Segment> segmentReader)
    {
        await segmentReader.WaitToReadAsync();
        return segmentReader.TryPeek(out var segment) && Matches(segment);
    }
}