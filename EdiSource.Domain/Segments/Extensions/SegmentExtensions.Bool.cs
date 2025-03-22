using EdiSource.Domain.Exceptions;

namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse a bool from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="trueValue">The value that is expected when true</param>
    /// <param name="compositeElement"></param>
    /// <param name="implicitFalse">
    ///     When true, if element is not trueValue then false.
    ///     When false, if element is not trueValue or falseValue then null
    /// </param>
    /// <param name="falseValue">The value expected when false</param>
    /// <returns></returns>
    public static bool? GetBool(this Segment it, int dataElement, string trueValue, int compositeElement = 0,
        bool implicitFalse = true,
        string falseValue = "N")
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return element == trueValue
            ? true
            : element == falseValue
                ? false
                : implicitFalse
                    ? false
                    : null;
    }

    /// <summary>
    ///     Attempts to parse a bool from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="trueValue">The value that is expected when true</param>
    /// <param name="compositeElement"></param>
    /// <param name="implicitFalse">
    ///     When true, if element is not trueValue then false.
    ///     When false, if element is not trueValue or falseValue then null
    /// </param>
    /// <param name="falseValue">The value expected when false</param>
    /// <returns></returns>
    public static bool GetBoolRequired(this Segment it, int dataElement, string trueValue, int compositeElement = 0,
        bool implicitFalse = true,
        string falseValue = "N")
    {
        return GetBool(it, dataElement, trueValue, compositeElement, implicitFalse, falseValue) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(bool));
    }

    /// <summary>
    ///     Attempts to set a bool from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="trueValue"></param>
    /// <param name="falseValue"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetBool(this Segment it, bool value, string trueValue, string falseValue, int dataElement,
        int compositeElement = 0, bool create = true)
    {
        var text = value ? trueValue : falseValue;
        return it.SetCompositeElement(text, dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set a bool from an element
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="trueValue"></param>
    /// <param name="falseValue"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns></returns>
    public static bool SetBool(this Segment it, bool? value, string trueValue, string falseValue, int dataElement,
        int compositeElement = 0, bool create = true)
    {
        if (value is null) return false;

        var text = value.Value ? trueValue : falseValue;
        return it.SetCompositeElement(text, dataElement, compositeElement, create);
    }
}