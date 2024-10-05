using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.IO.EdiReader;

public sealed class EdiParser<T> : IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    public async Task<T> ParseEdi(FileInfo fileInfo)
    {
        await using var fileStream = fileInfo.OpenRead();
        using var streamReader = new StreamReader(fileStream);

        var channel = Channel.CreateUnbounded<ISegment>();

        var loopInitializer = T.InitializeAsync(channel.Reader, null);

        await Task.WhenAll(
            new EdiReader().ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer),
            loopInitializer);

        return loopInitializer.Result;
    }

    public async Task<T> ParseEdi(string ediText)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);

        var channel = Channel.CreateUnbounded<ISegment>();

        var loopInitializer = T.InitializeAsync(channel.Reader, null);

        await Task.WhenAll(
            new EdiReader().ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer),
            loopInitializer);

        return loopInitializer.Result;
    }
}