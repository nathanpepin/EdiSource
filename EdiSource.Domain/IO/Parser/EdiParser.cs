using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.IO.EdiReader;

public sealed class EdiParser<T> : IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    public async Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null)
    {
        var channel = Channel.CreateUnbounded<ISegment>();

        var loopInitializer = T.InitializeAsync(channel.Reader, null);

        await Task.WhenAll(
            new EdiReader().ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer, separators),
            loopInitializer);

        return loopInitializer.Result;
    }

    public async Task<T> ParseEdi(FileInfo fileInfo, Separators? separators = null)
    {
        await using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream);
        return await ParseEdi(streamReader, separators);
    }

    public async Task<T> ParseEdi(string ediText, Separators? separators = null)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);
        return await ParseEdi(streamReader, separators);
    }

    public async Task<T> ParseEdiEnvelope(StreamReader streamReader)
    {
        var separators = await Separators.CreateFromISA(streamReader);
        return await ParseEdi(streamReader, separators);
    }

    public async Task<T> ParseEdiEnvelope(FileInfo fileInfo)
    {
        await using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream);
        return await ParseEdiEnvelope(streamReader);
    }

    public async Task<T> ParseEdiEnvelope(string ediText)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);
        return await ParseEdiEnvelope(streamReader);
    }
}