using System.Globalization;

namespace EdiSource.Domain.Elements;

public static class ElementExtensions
{
    public static DateTime? GetDate(this Element it, int compositeElement = 0, string format = "yyyyMMdd")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : DateTime.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : null;
    }

    public static int? GetInt(this Element it, int compositeElement = 0)
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : int.TryParse(element, out var value)
                ? value
                : null;
    }

    public static decimal? GetDecimal(this Element it, int compositeElement = 0)
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : decimal.TryParse(element, out var value)
                ? value
                : null;
    }

    public static bool? GetBool(this Element it, string trueValue, int compositeElement = 0, bool implicitFalse = true,
        string falseValue = "N")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;
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

    public static TEnum GetEnum<TEnum>(this Element it, int compositeElement = 0, bool tryAddUnderscore = true)
        where TEnum : struct, Enum
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? default
            : Enum.TryParse<TEnum>(element, out var value)
                ? value
                : tryAddUnderscore
                    ? Enum.TryParse($"_{element}", out value)
                        ? value
                        : default
                    : default;
    }

    public static DateOnly GetDateOnly(this Element it, int compositeElement = 0, string format = "yyyyMMdd")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? default
            : DateOnly.ParseExact(element, format, CultureInfo.InvariantCulture);
    }

    public static TimeOnly GetTimeOnly(this Element it, int compositeElement = 0, string format = "HHmm")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;
        return element is null
            ? default
            : TimeOnly.ParseExact(element, format, CultureInfo.InvariantCulture);
    }
}