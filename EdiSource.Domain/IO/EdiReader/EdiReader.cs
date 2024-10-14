using System.Buffers;
using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Exceptions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Structure;

namespace EdiSource.Domain.IO.EdiReader;

public sealed class EdiReader : IEdiReader
{
    private const int BufferSize = 4096;

    public BasicEdi ReadBasicEdi(string ediString)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediString));
        using var streamReader = new StreamReader(memoryStream);
        return ReadBasicEdi(streamReader);
    }

    public IEnumerable<ISegment> ReadEdiSegments(string ediString, Separators? separators = null)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediString));
        using var streamReader = new StreamReader(memoryStream);
        return ReadEdiSegments(streamReader, separators ?? Separators.DefaultSeparators);
    }

    public BasicEdi ReadBasicEdi(StreamReader streamReader)
    {
        return ReadBasicEdiAsync(streamReader).GetAwaiter().GetResult();
    }

    public IEnumerable<ISegment> ReadEdiSegments(StreamReader streamReader, Separators? separators = null)
    {
        return ReadEdiSegmentsAsync(streamReader, separators ?? Separators.DefaultSeparators).GetAwaiter().GetResult();
    }

    public async Task<BasicEdi> ReadBasicEdiAsync(StreamReader streamReader,
        CancellationToken cancellationToken = default)
    {
        var separators = await Separators.CreateFromISA(streamReader);
        var segments = await ReadEdiSegmentsAsync(streamReader, separators, cancellationToken);
        return new BasicEdi(segments.ToArray(), separators);
    }

    public async Task<IEnumerable<ISegment>> ReadEdiSegmentsAsync(StreamReader streamReader,
        Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        var channel = Channel.CreateBounded<ISegment>(1);

        List<ISegment> segments = [];

        _ = Task.Run(
            async () => await ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer, separators,
                cancellationToken), cancellationToken);

        await foreach (var segment in channel.Reader.ReadAllAsync(cancellationToken)) segments.Add(segment);

        return segments;
    }

    public async Task ReadEdiSegmentsIntoChannelAsync(StreamReader streamReader,
        ChannelWriter<ISegment> channelWriter,
        Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        separators ??= Separators.DefaultSeparators;

        Segment segmentBuffer = new([]);

        StringBuilder stringBuffer = new();

        var buffer = ArrayPool<char>.Shared.Rent(BufferSize);

        var segmentsCreated = 0;

        try
        {
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            streamReader.DiscardBufferedData();

            while (streamReader.Peek() > 0)
            {
                await streamReader.ReadAsync(buffer, 0, BufferSize);

                for (var i = 0; i < BufferSize; i++)
                {
                    if (buffer[i] is '\r' or '\n' or '\0') continue;

                    if (buffer[i] == separators.SegmentSeparator)
                    {
                        segmentBuffer
                            .Elements
                            .Last()
                            .Add(stringBuffer.ToString());

                        stringBuffer.Clear();

                        await channelWriter.WriteAsync(segmentBuffer, cancellationToken);
                        segmentsCreated++;

                        segmentBuffer = new Segment([])
                        {
                            Separators = separators
                        };
                    }
                    else if (buffer[i] == separators.DataElementSeparator)
                    {
                        segmentBuffer
                            .Elements
                            .Last()
                            .Add(stringBuffer.ToString());

                        stringBuffer.Clear();

                        segmentBuffer.Elements.Add([]);
                    }
                    else if (buffer[i] == separators.CompositeElementSeparator)
                    {
                        segmentBuffer
                            .Elements
                            .Last()
                            .Add(stringBuffer.ToString());

                        stringBuffer.Clear();
                    }
                    else
                    {
                        if (segmentBuffer.Elements.Count == 0)
                            segmentBuffer.Elements.Add([]);

                        stringBuffer.Append(buffer[i]);
                    }
                }
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
            channelWriter.Complete();
        }

        if (segmentsCreated == 0)
            throw new EdiReaderException();
    }
}