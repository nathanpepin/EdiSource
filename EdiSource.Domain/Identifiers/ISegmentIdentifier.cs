using System.Threading.Channels;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Used to identify a segment
/// </summary>
public interface ISegmentIdentifier
{
    static abstract (string Primary, string? Secondary) EdiId { get; }
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
    public static bool Matches(ISegment segment)
    {
        return (T.EdiId.Primary, T.EdiId.Secondary) switch
        {
            (not null, null) => segment.GetDataElement(0) == T.EdiId.Primary,
            (not null, not null) =>
                segment.GetDataElement(0) == T.EdiId.Primary &&
                segment.GetCompositeElementOrNull(1, 0) == T.EdiId.Secondary,
            _ => false
        };
    }

    /// <summary>
    ///     Matches the first item from a Queue using the segment identifiers
    /// </summary>
    /// <param name="segments"></param>
    /// <returns></returns>
    public static bool Matches(Queue<ISegment> segments)
    {
        return segments.Count > 0 &&
               segments.Peek().GetDataElement(0) == T.EdiId.Primary &&
               (T.EdiId.Secondary is null
                || T.EdiId.Secondary == segments.Peek().GetCompositeElementOrNull(0, 0));
    }

    /// <summary>
    ///     Matches the first item from a Channel using the segment identifiers
    /// </summary>
    /// <param name="segmentReader"></param>
    /// <returns></returns>
    public static async ValueTask<bool> MatchesAsync(ChannelReader<ISegment> segmentReader)
    {
        await segmentReader.WaitToReadAsync();
        return segmentReader.TryPeek(out var segment) && Matches(segment);
    }
}