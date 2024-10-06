using System.Globalization;

namespace EdiSource.Domain.Elements;

public static class ElementExtensions
{
    /// <summary>
    /// Attempts to parse a date from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to parse an int from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to parse a decimal from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to parse a bool from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="trueValue">The value that is expected when true</param>
    /// <param name="compositeElement"></param>
    /// <param name="implicitFalse">When true, if element is not trueValue then false.
    /// When false, if element is not trueValue or falseValue then null</param>
    /// <param name="falseValue">The value expected when false</param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to parse an enum from an element via enum name.
    ///
    /// If tryAddUnderscore is true, the will also try an enum name with
    /// an '_' prefix to enable enums that start with not allowed characters.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <param name="tryAddUnderscore"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum? GetEnum<TEnum>(this Element it, int compositeElement = 0, bool tryAddUnderscore = true)
        where TEnum : struct, Enum
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : Enum.TryParse<TEnum>(element, out var value)
                ? value
                : tryAddUnderscore
                    ? Enum.TryParse($"_{element}", out value)
                        ? value
                        : null
                    : null;
    }

    /// <summary>
    /// Attempts to parse a DateOnly from element.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateOnly? GetDateOnly(this Element it, int compositeElement = 0, string format = "yyyyMMdd")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : DateOnly.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : null;
    }

    /// <summary>
    /// Attempts to parse a TimeOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static TimeOnly? GetTimeOnly(this Element it, int compositeElement = 0, string format = "HHmm")
    {
        var element = it.Count > compositeElement
            ? it[compositeElement]
            : null;

        return element is null
            ? null
            : TimeOnly.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
                ? time
                : null;
    }
}