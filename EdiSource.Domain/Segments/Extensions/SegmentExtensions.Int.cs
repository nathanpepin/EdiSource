using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse an int from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
    public static int? GetInt(this Segment it, int dataElement, int compositeElement = 0)
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return int.TryParse(element, out var value)
            ? value
            : null;
    }

    /// <summary>
    ///     Attempts to parse an int from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <returns></returns>
    public static int GetIntRequired(this Segment it, int dataElement, int compositeElement = 0)
    {
        return GetInt(it, dataElement, compositeElement) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(int));
    }

    /// <summary>
    ///     Attempts to set an int from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetInt(this Segment it, int value, int dataElement, int compositeElement = 0,
        string? format = null, bool create = true)
    {
        var text = format is null ? value.ToString() : value.ToString(format);
        return it.SetCompositeElement(text, dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set an int from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="format"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetInt(this Segment it, int? value, int dataElement, int compositeElement = 0,
        string? format = null, bool create = true)
    {
        if (value is null) return false;

        var text = format is null ? value.Value.ToString() : value.Value.ToString(format);
        return it.SetCompositeElement(text, dataElement, compositeElement, create);
    }
}