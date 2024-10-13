using System.Globalization;
using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse a DateOnly from element.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateOnly? GetDateOnly(this ISegment it, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd")
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return DateOnly.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    /// <summary>
    ///     Attempts to parse a DateOnly from element.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateOnly GetDateOnlyRequired(this ISegment it, int dataElement, int compositeElement = 0,
        string format = "yyyyMMdd")
    {
        return GetDateOnly(it, dataElement, compositeElement, format) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(DateOnly));
    }

    /// <summary>
    /// 
    ///     Attempts to set a DateOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDateOnly(this ISegment it, DateOnly value, int dataElement,
        int compositeElement = 0, string format = "yyyyMMdd", bool create = true)
    {
        return it.SetCompositeElement(dataElement, compositeElement, value.ToString(format), create);
    }

    /// <summary>
    ///     Attempts to set a DateOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDateOnly(this ISegment it, DateOnly? value, int dataElement,
        int compositeElement = 0, string format = "yyyyMMdd", bool create = true)
    {
        return value is not null
               && it.SetCompositeElement(dataElement, compositeElement, value.Value.ToString(format), create);
    }
}