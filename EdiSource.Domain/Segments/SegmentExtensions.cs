using System.Globalization;
using System.Text;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Segments;

public static class SegmentExtensions
{
    public static DateTime? GetDate(this ISegment segment, int elementIndex, int compositeElement = 0, string format = "yyyyMMdd")
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? null
            : DateTime.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : null;
    }

    public static int? GetInt(this ISegment segment, int elementIndex, int compositeElement = 0)
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? null
            : int.TryParse(element, out var value)
                ? value
                : null;
    }

    public static decimal? GetDecimal(this ISegment segment, int elementIndex, int compositeElement = 0)
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? null
            : decimal.TryParse(element, out var value)
                ? value
                : null;
    }

    public static bool? GetBool(this ISegment segment, string trueValue, int elementIndex, int compositeElement = 0, bool implicitFalse = true, string falseValue = "N")
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? null
            : element == trueValue
                ? true
                : element == falseValue
                    ? false
                    : implicitFalse
                        ? false
                        : null;
    }

    public static TEnum GetEnum<TEnum>(this ISegment segment, int elementIndex, int compositeElement = 0, bool tryAddUnderscore = true) where TEnum : struct, Enum
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? default
            : Enum.TryParse<TEnum>(element, out var value)
                ? value
                : tryAddUnderscore
                    ? Enum.TryParse<TEnum>($"_{element}", out value)
                        ? value
                        : default
                    : default;
    }

    public static DateOnly GetDateOnly(this ISegment segment, int elementIndex, int compositeElement = 0, string format = "yyyyMMdd")
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? default
            : DateOnly.ParseExact(element, format, CultureInfo.InvariantCulture);
    }

    public static TimeOnly GetTimeOnly(this ISegment segment, int elementIndex, int compositeElement = 0, string format = "HHmm")
    {
        var element = segment.GetCompositeElementOrNull(elementIndex, compositeElement);
        return element is null
            ? default
            : TimeOnly.ParseExact(element, format, CultureInfo.InvariantCulture);
    }

    public static StringBuilder WriteToStringBuilder<T>(this T segment, StringBuilder? stringBuilder = null, Separators separators = default)
        where T : ISegment
    {
        stringBuilder ??= new StringBuilder();

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