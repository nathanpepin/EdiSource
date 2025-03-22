using System.Globalization;
using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse a TimeOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static TimeOnly? GetTimeOnly(this Segment it, int dataElement, int compositeElement = 0,
        string format = "HHmm")
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return TimeOnly.TryParseExact(element, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
            ? time
            : null;
    }

    /// <summary>
    ///     Attempts to parse a TimeOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static TimeOnly GetTimeOnlyRequired(this Segment it, int dataElement, int compositeElement = 0,
        string format = "HHmm")
    {
        return GetTimeOnly(it, dataElement, compositeElement, format) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(TimeOnly));
    }

    /// <summary>
    ///     Attempts to set a TimeOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetTimeOnly(this Segment it, TimeOnly value, int dataElement,
        int compositeElement = 0, string format = "HHmm", bool create = true)
    {
        return it.SetCompositeElement(value.ToString(format), dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set a TimeOnly from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetTimeOnly(this Segment it, TimeOnly? value, int dataElement,
        int compositeElement = 0, string format = "HHmm", bool create = true)
    {
        return value is not null &&
               it.SetCompositeElement(value.Value.ToString(format), dataElement, compositeElement, create);
    }
}