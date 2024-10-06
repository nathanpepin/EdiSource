using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Parser;

public sealed class EdiParser<T> : IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    public async Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        var channel = Channel.CreateUnbounded<ISegment>();

        var loopInitializer = T.InitializeAsync(channel.Reader, null);

        await Task.WhenAll(
            new EdiReader.EdiReader()
                .ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer, separators, cancellationToken),
            loopInitializer);

        return loopInitializer.Result;
    }

    public async Task<T> ParseEdi(FileInfo fileInfo, Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        await using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream);
        return await ParseEdi(streamReader, separators, cancellationToken);
    }

    public async Task<T> ParseEdi(string ediText, Separators? separators = null)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);
        return await ParseEdi(streamReader, separators);
    }

    public async Task<T> ParseEdiEnvelope(StreamReader streamReader, CancellationToken cancellationToken = default)
    {
        var separators = await Separators.CreateFromISA(streamReader);
        return await ParseEdi(streamReader, separators, cancellationToken);
    }

    public async Task<T> ParseEdiEnvelope(FileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        await using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream);
        return await ParseEdiEnvelope(streamReader, cancellationToken);
    }

    public async Task<T> ParseEdiEnvelope(string ediText)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);
        return await ParseEdiEnvelope(streamReader);
    }
}