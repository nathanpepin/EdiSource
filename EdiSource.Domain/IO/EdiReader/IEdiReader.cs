using System.Threading.Channels;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;
using EdiSource.Domain.Structure;

namespace EdiSource.Domain.IO.EdiReader;

public interface IEdiReader
{
    BasicEdi ReadBasicEdi(string ediString);
    BasicEdi ReadBasicEdi(StreamReader streamReader);
    Task<BasicEdi> ReadBasicEdiAsync(StreamReader streamReader, CancellationToken cancellationToken = default);
    IEnumerable<ISegment> ReadEdiSegments(string ediString, Separators? separators = null);
    IEnumerable<ISegment> ReadEdiSegments(StreamReader streamReader, Separators? separators = null);

    Task<IEnumerable<ISegment>> ReadEdiSegmentsAsync(StreamReader streamReader, Separators? separators = null,
        CancellationToken cancellationToken = default);

    Task ReadEdiSegmentsIntoChannelAsync(StreamReader streamReader,
        ChannelWriter<Segment> channelWriter,
        Separators? separators = null,
        CancellationToken cancellationToken = default);
}