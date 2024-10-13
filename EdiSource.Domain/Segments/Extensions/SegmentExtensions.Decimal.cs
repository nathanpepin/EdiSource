using System.Globalization;
using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse a decimal from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
    public static decimal? GetDecimal(this ISegment it, int dataElement, int compositeElement = 0)
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return decimal.TryParse(element, out var value)
            ? value
            : null;
    }

    /// <summary>
    ///     Attempts to parse a decimal from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
    public static decimal GetDecimalRequired(this ISegment it, int dataElement, int compositeElement = 0)
    {
        return GetDecimal(it, dataElement, compositeElement) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(decimal));
    }

    /// <summary>
    ///     Attempts to set a decimal from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDecimal(this ISegment it, decimal value, int dataElement, int compositeElement = 0,
        string? format = null, bool create = true)
    {
        var text = format is null ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(format);
        return it.SetCompositeElement(dataElement, compositeElement, text, create);
    }

    /// <summary>
    ///     Attempts to set a decimal from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetDecimal(this ISegment it, decimal? value, int dataElement, int compositeElement = 0,
        string? format = null, bool create = true)
    {
        if (value is null) return false;
        var text = format is null ? value.Value.ToString(CultureInfo.InvariantCulture) : value.Value.ToString(format);
        return it.SetCompositeElement(dataElement, compositeElement, text, create);
    }
}