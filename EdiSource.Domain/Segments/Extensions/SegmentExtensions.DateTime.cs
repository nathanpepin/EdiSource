using System.Globalization;
using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse a date from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateTime? GetDate(this Segment it, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd")
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return DateTime.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    /// <summary>
    ///     Attempts to parse a date from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateTime GetDateRequired(this Segment it, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd")
    {
        return GetDate(it, dataElement, compositeElement, format) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(DateTime));
    }

    /// <summary>
    ///     Attempts to set a date from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDate(this Segment it, DateTime value, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd", bool create = true)
    {
        return it.SetCompositeElement(value.ToString(format), dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set a date from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDate(this Segment it, DateTime? value, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd", bool create = true)
    {
        return value is not null
               && it.SetCompositeElement(value.Value.ToString(format), dataElement, compositeElement, create);
    }
}