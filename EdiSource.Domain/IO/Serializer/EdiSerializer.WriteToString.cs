using System.Text;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public sealed partial class EdiSerializer
{
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