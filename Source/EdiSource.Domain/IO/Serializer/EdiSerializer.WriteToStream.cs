namespace EdiSource.Domain.IO.Serializer;

public sealed partial class EdiSerializer
{
    public async Task WriteToStream(ILoop loop, Stream stream, Separators? separators = null,
        bool includeNewLine = true, bool leaveOpen = true, CancellationToken cancellationToken = default)
    {
        separators ??= Separators.DefaultSeparators;

        await using var writer = new StreamWriter(stream, leaveOpen: leaveOpen);

        foreach (var segment in loop.YieldChildSegments())
        {
            var text = segment.WriteToStringBuilder(separators: separators);

            if (includeNewLine)
                await writer.WriteLineAsync(text, cancellationToken);
            else
                await writer.WriteAsync(text, cancellationToken);
        }
    }
}