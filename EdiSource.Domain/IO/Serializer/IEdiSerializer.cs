using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public interface IEdiSerializer
{
    Task WriteToStream(ILoop loop, Stream stream, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default);

    Task WriteToFile(ILoop loop, FileInfo fileInfo, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default);

    string WriteToString(ILoop loop, Separators? separators = null,
        bool includeNewLine = true);

    string WriteToPrettyString(ILoop loop, Separators? separators = null);
}