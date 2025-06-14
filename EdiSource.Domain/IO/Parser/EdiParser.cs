using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Exceptions;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Standard.Loops.ISA;

namespace EdiSource.Domain.IO.Parser;

public sealed class EdiParser<T> : IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    public async Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        if (typeof(T) == typeof(InterchangeEnvelope) && separators is null)
        {
            if (await Separators.IsInvalidISA(streamReader) is { } exception)
            {
                throw exception;
            }

            separators ??= await Separators.CreateFromISA(streamReader);
        }

        var channel = Channel.CreateUnbounded<Segment>();

        var loopInitializer = T.InitializeAsync(channel.Reader, null);

        await Task.WhenAll(
            new EdiReader.EdiReader()
                .ReadEdSegmentsIntoChannelAsync(streamReader, channel.Writer, separators, cancellationToken),
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

    public async Task<T> ParseEdi(string ediText, Separators? separators = null, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediText));
        using var streamReader = new StreamReader(memoryStream);
        return await ParseEdi(streamReader, separators, cancellationToken);
    }
}