using System.Threading.Channels;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Structure;

namespace EdiSource.Domain.IO.EdiReader;

public interface IEdiReader
{
    /// <summary>
    /// Converts text to a BasicEdi format. Must be in envelope form.
    /// </summary>
    /// <param name="ediString"></param>
    /// <returns></returns>
    BasicEdi ReadBasicEdi(string ediString);

    /// <summary>
    /// Converts a stream into BasicEdi format. Must be in envelope form.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <returns></returns>
    BasicEdi ReadBasicEdi(StreamReader streamReader);

    /// <summary>
    /// Converts a stream into BasicEdi format. Must be in envelope form.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BasicEdi> ReadBasicEdiAsync(StreamReader streamReader, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Converts text into a list of segments.
    /// </summary>
    /// <param name="ediString"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <returns></returns>
    IEnumerable<ISegment> ReadEdiSegments(string ediString, Separators? separators = null);

    /// <summary>
    ///  Converts a stream into a list of segments.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <returns></returns>
    IEnumerable<ISegment> ReadEdiSegments(StreamReader streamReader, Separators? separators = null);

    /// <summary>
    ///  Converts a stream into a list of segments.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<ISegment>> ReadEdiSegmentsAsync(StreamReader streamReader, Separators? separators = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts contents from a stream into segments and pushes results to a channel
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="channelWriter"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ReadEdiSegmentsIntoChannelAsync(StreamReader streamReader,
        ChannelWriter<ISegment> channelWriter,
        Separators? separators = null,
        CancellationToken cancellationToken = default);
}