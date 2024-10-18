using System.Text;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    public static StringBuilder WriteToStringBuilder<T>(this T segment, StringBuilder? stringBuilder = null,
        Separators? separators = default)
        where T : ISegment
    {
        stringBuilder ??= new StringBuilder();
        separators ??= Separators.DefaultSeparators;
        
        if (segment.Elements.Count == 0)
            return stringBuilder;

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