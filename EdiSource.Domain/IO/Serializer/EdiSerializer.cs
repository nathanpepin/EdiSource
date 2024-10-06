using System.Text;
using EdiSource.Domain.Helper;
using EdiSource.Domain.Helper.PrettyPrinting;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public sealed partial class EdiSerializer : IEdiSerializer
{
    public async Task WriteToStream(ILoop loop, Stream stream, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default)
    {
        separators ??= Separators.DefaultSeparators;

        await using var writer = new StreamWriter(stream);

        foreach (var segment in loop.YieldChildSegments())
        {
            var text = segment.WriteToStringBuilder(separators: separators);

            if (includeNewLine)
                await writer.WriteLineAsync(text, cancellationToken);
            else
                await writer.WriteAsync(text, cancellationToken);
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
}