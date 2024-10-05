using System.Text;
using EdiSource.Domain.Helper;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public sealed class EdiSerializer : IEdiSerializer
{
    public async Task WriteToStream(ILoop loop, Stream stream, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default)
    {
        separators ??= Separators.DefaultSeparators;

        await using var writer = new StreamEdiWriter(stream);

        foreach (var segment in loop.YieldChildSegments())
        {
            var text = segment.WriteToStringBuilder(separators: separators).ToString();
            await writer.WriteTextAsync(text, cancellationToken);

            if (includeNewLine)
                await writer.WriteTextAsync(Environment.NewLine, cancellationToken);
        }
    }

    public async Task WriteToFile(ILoop loop, FileInfo fileInfo, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default)
    {
        await using var stream = fileInfo.OpenWrite();
        await WriteToStream(loop, stream, separators, includeNewLine, cancellationToken);
    }

    public string WriteToString(ILoop loop, Separators? separators = null,
        bool includeNewLine = true)
    {
        separators ??= Separators.DefaultSeparators;
        var stringBuilder = new StringBuilder();

        foreach (var segment in loop.YieldChildSegments())
        {
            segment.WriteToStringBuilder(stringBuilder, separators);

            if (includeNewLine)
                stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    public string WriteToPrettyString(ILoop loop, Separators? separators = null)
    {
        return PrettyPrintToStringBuilder(loop, separators: separators).ToString();

        static LoopPrinter PrettyPrintToStringBuilder(ILoop loop, LoopPrinter? loopPrinter = null,
            Separators? separators = null, bool firstIteration = true)
        {
            separators ??= Separators.DefaultSeparators;
            loopPrinter ??= new LoopPrinter();

            var loopHeader = firstIteration
                ? loopPrinter.AppendLoop(loop.GetType().Name)
                : null;

            loop.EdiAction(x =>
                {
                    var text = x.WriteToStringBuilder(separators: separators).ToString();
                    loopPrinter.AppendLine(text);
                },
                segmentList =>
                {
                    foreach (var text in segmentList
                                 .Select(segment => segment.WriteToStringBuilder(separators: separators).ToString()))
                        loopPrinter.AppendLine(text);
                },
                loopL =>
                {
                    var loopText = loopL.GetType().Name;
                    using var d = loopPrinter.AppendLoop(loopText);
                    PrettyPrintToStringBuilder(loopL, loopPrinter, separators, false);
                },
                loopList =>
                {
                    foreach (var loopL in loopList)
                    {
                        var loopText = loopL.GetType().Name;
                        using var d = loopPrinter.AppendLoop(loopText);
                        PrettyPrintToStringBuilder(loopL, loopPrinter, separators, false);
                    }
                });

            loopHeader?.Dispose();

            return loopPrinter;
        }
    }
}