using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Elements;

/// <summary>
///     An element presents a data element.
///     A data element is comprised of composite elements, though typically most data elements only contain a single
///     composite element.
///     Because of this, the class contains overloads and implicit typing to allow for easier use.
///     Element element = "20230101";
///     Element element2 = ["20230101", "20240101"];
/// </summary>
/// <param name="values"></param>
public sealed partial class Element(IEnumerable<string>? values = null) : List<string>
{
    private readonly List<string> _compositeElements = values?.ToList() ?? [];

    public static implicit operator Element(string[] values)
    {
        return new Element(values);
    }

    public static implicit operator Element(string value)
    {
        return new Element([value]);
    }

    public static implicit operator string(Element element)
    {
        return element._compositeElements[0];
    }

    public static implicit operator string[](Element element)
    {
        return element._compositeElements.ToArray();
    }

    /// <summary>
    ///     Create a segment equivalent from segment text
    /// </summary>
    /// <param name="segmentText"></param>
    /// <param name="separators"></param>
    /// <returns></returns>
    public static Element[] FromString(string segmentText, Separators? separators = null)
    {
        separators ??= Separators.DefaultSeparators;

        return segmentText
            .Trim()
            .Trim(separators.SegmentSeparator)
            .Split(separators.DataElementSeparator)
            .Select(x => x.Split(separators.CompositeElementSeparator))
            .Select(x => new Element(x))
            .ToArray();
    }

    /// <summary>
    ///     Creates multiple segment equivalents from segment text
    /// </summary>
    /// <param name="segmentText"></param>
    /// <param name="separators"></param>
    /// <returns></returns>
    public static Element[][] MultipleFromString(string segmentText, Separators? separators = null)
    {
        separators ??= Separators.DefaultSeparators;

        return segmentText
            .Split(separators.SegmentSeparator)
            .Select(x => FromString(segmentText, separators))
            .ToArray();
    }
}