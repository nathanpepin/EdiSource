using System.Text;
using EdiSource.Domain.Helper;
using EdiSource.Domain.Helper.PrettyPrinting;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public sealed partial class EdiSerializer : IEdiSerializer
{
    public async Task WriteToFile(ILoop loop, FileInfo fileInfo, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default)
    {
        await using var stream = fileInfo.OpenWrite();
        await WriteToStream(loop, stream, separators, includeNewLine, cancellationToken);
    }
}