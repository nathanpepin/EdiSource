using System.Text;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments.Extensions;

public static class SegmentExtensions
{
    public static StringBuilder WriteToStringBuilder<T>(this T segment, StringBuilder? stringBuilder = null,
        Separators? separators = default)
        where T : ISegment
    {
        stringBuilder ??= new StringBuilder();
        separators ??= Separators.DefaultSeparators;

        foreach (var element in segment.Elements.SkipLast(1))
        {
            stringBuilder.AppendJoin(separators.CompositeElementSeparator, (string[])element);
            stringBuilder.Append(separators.DataElementSeparator);
        }

        stringBuilder.AppendJoin(separators.CompositeElementSeparator, (string[])segment.Elements.Last());
        stringBuilder.Append(separators.SegmentSeparator);

        return stringBuilder;
    }
}