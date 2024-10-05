using System.Threading.Channels;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

public interface ISegmentIdentifier
{
    static abstract (string Primary, string? Secondary) EdiId { get; }
}

public interface ISegmentIdentifier<T> : ISegmentIdentifier
    where T : ISegmentIdentifier<T>
{
    public static bool Matches(ISegment segment)
    {
        return segment.GetDataElement(0) == T.EdiId.Primary &&
            T.EdiId.Secondary is null || T.EdiId.Secondary == segment.GetCompositeElementOrNull(0, 0);
    }

    public static bool Matches(Queue<ISegment> segments)
    {
        return segments.Count > 0 &&
               segments.Peek().GetDataElement(0) == T.EdiId.Primary &&
               (T.EdiId.Secondary is null
                || T.EdiId.Secondary == segments.Peek().GetCompositeElementOrNull(0, 0));
    }

    public static async ValueTask<bool> MatchesAsync(ChannelReader<ISegment> segmentReader)
    {
        if (!segmentReader.TryPeek(out var segment))
        {
            return await segmentReader.WaitToReadAsync() && await MatchesAsync(segmentReader);
        }

        return Matches(segment);
    }
}

public interface ISegmentId : ISegment, ISegmentIdentifier;

public interface IEdiId : IEdi, ISegmentIdentifier;