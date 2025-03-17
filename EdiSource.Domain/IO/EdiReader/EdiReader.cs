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

    public IEnumerable<Segment> ReadEdSegments(string ediString, Separators? separators = null)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ediString));
        using var streamReader = new StreamReader(memoryStream);
        return ReadEdSegments(streamReader, separators ?? Separators.DefaultSeparators);
    }

    public BasicEdi ReadBasicEdi(StreamReader streamReader)
    {
        return ReadBasicEdiAsync(streamReader).GetAwaiter().GetResult();
    }

    public IEnumerable<Segment> ReadEdSegments(StreamReader streamReader, Separators? separators = null)
    {
        return ReadEdSegmentsAsync(streamReader, separators ?? Separators.DefaultSeparators).GetAwaiter().GetResult();
    }

    public async Task<BasicEdi> ReadBasicEdiAsync(StreamReader streamReader,
        CancellationToken cancellationToken = default)
    {
        var separators = await Separators.CreateFromISA(streamReader);
        var segments = await ReadEdSegmentsAsync(streamReader, separators, cancellationToken);
        return new BasicEdi(segments.ToArray(), separators);
    }

    public async Task<List<Segment>> ReadEdSegmentsAsync(StreamReader streamReader,
        Separators? separators = null,
        CancellationToken cancellationToken = default)
    {
        var channel = Channel.CreateBounded<Segment>(1);

        List<Segment> segments = [];

        _ = Task.Run(
            async () => await ReadEdSegmentsIntoChannelAsync(streamReader, channel.Writer, separators,
                cancellationToken), cancellationToken);

        await foreach (var segment in channel.Reader.ReadAllAsync(cancellationToken)) segments.Add(segment);

        return segments;
    }

    public async Task ReadEdSegmentsIntoChannelAsync(StreamReader streamReader,
        ChannelWriter<Segment> channelWriter,
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
                    var c = buffer[i];
                    if (c is '\r' or '\n' or '\0') continue;

                    if (c == separators.SegmentSeparator)
                    {
                        try
                        {
                            if (segmentBuffer.Elements.Count == 0)
                            {
                                throw new EmptySegmentException(separators.SegmentSeparator, segmentsCreated);
                            }

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
                        catch (EmptySegmentException)
                        {
                            throw;
                        }
                        catch (Exception exception)
                        {
                            throw new EdiSegmentReaderException(segmentBuffer.ToString(), segmentsCreated, exception);
                        }
                    }
                    else if (c == separators.DataElementSeparator)
                    {
                        try
                        {
                            segmentBuffer
                                .Elements
                                .Last()
                                .Add(stringBuffer.ToString());

                            stringBuffer.Clear();

                            segmentBuffer.Elements.Add([]);
                        }
                        catch (Exception exception)
                        {
                            throw new EdiSegmentReaderException(segmentBuffer.ToString(), segmentsCreated, exception);
                        }
                    }

                    else if (c == separators.CompositeElementSeparator)
                    {
                        try
                        {
                            segmentBuffer
                                .Elements
                                .Last()
                                .Add(stringBuffer.ToString());

                            stringBuffer.Clear();
                        }
                        catch (Exception exception)
                        {
                            throw new EdiSegmentReaderException(segmentBuffer.ToString(), segmentsCreated, exception);
                        }
                    }
                    else
                    {
                        if (segmentBuffer.Elements.Count == 0)
                            segmentBuffer.Elements.Add([]);

                        stringBuffer.Append(c);
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