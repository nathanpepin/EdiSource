using EdiSource.Domain.Elements;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments;

/// <summary>
///     A basic implementation of ISegment
/// </summary>
public class Segment : ISegment
{
    private Separators? _separators;

    public Segment(ISegment segment, ILoop? parent = null)
    {
        Elements = segment.Elements;
        Separators = segment.Separators;
        Parent = parent;
    }

    public Segment(IEnumerable<Element>? elements = null, Separators? separators = default, ILoop? parent = null)
    {
        Elements = elements?.ToList() ?? [];
        Separators = separators ?? Separators.DefaultSeparators;
        Parent = parent;
    }

    public Segment(string segmentText, Separators? separators = null, ILoop? parent = null)
    {
        Elements = ReadElements(segmentText, separators).ToList();
        Separators = separators ?? Separators.DefaultSeparators;
        Parent = parent;
    }

    public string this[int index]
    {
        get => this[index, 0];
        set => this[index, 0] = value;
    }

    public string this[int dataElement, int compositeElement]
    {
        get => GetCompositeElement(dataElement, compositeElement);
        set => SetCompositeElement(dataElement, compositeElement, value);
    }

    public ILoop? Parent { get; set; }

    public IList<Element> Elements { get; set; }

    public Element GetElement(int elementIndex)
    {
        return Elements[elementIndex];
    }

    public Element? GetElementOrNull(int elementIndex)
    {
        return ElementExists(elementIndex)
            ? GetElement(elementIndex)
            : null;
    }

    public string GetCompositeElement(int dataElementIndex, int compositeElementIndex)
    {
        return Elements[dataElementIndex][compositeElementIndex];
    }

    public string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex)
    {
        return CompositeElementExists(dataElementIndex, compositeElementIndex)
            ? GetCompositeElement(dataElementIndex, compositeElementIndex)
            : null;
    }

    public bool SetDataElement(int elementIndex, params string[] values)
    {
        if (!ElementExists(elementIndex)) return false;

        var element = GetElement(elementIndex);

        element.Clear();

        foreach (var value in values)
            element.Add(value);

        return true;
    }

    public bool SetCompositeElement(int dataElementIndex, int compositeElementIndex, string value)
    {
        if (!CompositeElementExists(dataElementIndex, compositeElementIndex)) return false;

        Elements[dataElementIndex][compositeElementIndex] = value;
        return true;
    }

    public bool ElementExists(int elementIndex)
    {
        return elementIndex >= Elements.Count || elementIndex < 0;
    }

    public bool CompositeElementExists(int dataElementIndex, int compositeElementIndex)
    {
        return (ElementExists(dataElementIndex) &&
                compositeElementIndex >= Elements[dataElementIndex].Count) || compositeElementIndex < 0;
    }

    public Separators Separators
    {
        get => _separators ?? Separators.DefaultSeparators;
        set => _separators = value;
    }

    public static IEnumerable<Segment> ReadMultipleSegment(string segmentText, Separators? separators = null)
    {
        separators ??= Separators.DefaultSeparators;

        return segmentText
            .Trim()
            .Split(separators.SegmentSeparator)
            .Select(x => new Segment(ReadElements(x, separators), separators));
    }

    public static IEnumerable<Element> ReadElements(string segmentText, Separators? separators = null)
    {
        separators ??= Separators.DefaultSeparators;

        return segmentText
            .Trim()
            .Trim(separators.SegmentSeparator)
            .Split(separators.DataElementSeparator)
            .Select(x => x.Split(separators.CompositeElementSeparator))
            .Select(x => new Element(x));
    }

    public override string ToString()
    {
        return this.WriteToStringBuilder(separators: Separators).ToString();
    }

    public string ToString(Separators separators)
    {
        return this.WriteToStringBuilder(separators: separators).ToString();
    }
}