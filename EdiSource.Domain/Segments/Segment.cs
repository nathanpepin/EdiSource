using EdiSource.Domain.Elements;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments;

/// <summary>
///     A basic implementation of Segment
/// </summary>
public class Segment
{
    private Separators? _separators;

    public Segment(Segment segment, ILoop? parent = null)
    {
        Elements = segment.Elements;
        Separators = segment.Separators;
    }

    public Segment(IEnumerable<Element>? elements = null, Separators? separators = default, ILoop? parent = null)
    {
        Elements = elements?.ToList() ?? [];
        Separators = separators ?? Separators.DefaultSeparators;
    }

    public Segment(string segmentText, Separators? separators = null, ILoop? parent = null)
    {
        Elements = ReadElements(segmentText, separators).ToList();
        Separators = separators ?? Separators.DefaultSeparators;
    }

    public string this[int index]
    {
        get => this[index, 0];
        set => this[index, 0] = value;
    }

    public string this[int dataElement, int compositeElement]
    {
        get => GetCompositeElement(dataElement, compositeElement);
        set => SetCompositeElement(value, dataElement, compositeElement);
    }

    public IList<Element> Elements { get; set; }

    public Separators Separators
    {
        get => _separators ?? Separators.DefaultSeparators;
        set => _separators = value;
    }

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

    public string GetCompositeElement(int dataElementIndex, int compositeElementIndex = 0)
    {
        return Elements[dataElementIndex][compositeElementIndex];
    }

    public string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex = 0)
    {
        return CompositeElementExists(dataElementIndex, compositeElementIndex)
            ? GetCompositeElement(dataElementIndex, compositeElementIndex)
            : null;
    }

    public bool SetDataElement(int elementIndex, bool create = true, params string[] values)
    {
        if (create && !ElementExists(elementIndex))
            while (elementIndex >= Elements.Count)
                Elements.Add([]);
        else if (!ElementExists(elementIndex)) return false;

        var element = GetElement(elementIndex);

        element.Clear();

        foreach (var value in values)
            element.Add(value);

        return true;
    }

    public bool SetCompositeElement(string value, int dataElementIndex, int compositeElementIndex = 0,
        bool create = true)
    {
        if (create && !CompositeElementExists(dataElementIndex, compositeElementIndex))
            while (dataElementIndex >= Elements.Count)
                if (dataElementIndex == Elements.Count - 1)
                    for (var i = 0; i < compositeElementIndex; i++)
                        Elements[dataElementIndex].Add(string.Empty);
                else
                    Elements.Add([string.Empty]);
        else if (!CompositeElementExists(dataElementIndex, compositeElementIndex)) return false;

        Elements[dataElementIndex][compositeElementIndex] = value;
        return true;
    }

    public bool ElementExists(int elementIndex)
    {
        return Elements.InsideBounds(elementIndex);
    }

    public bool CompositeElementExists(int dataElementIndex, int compositeElementIndex = 0)
    {
        return Elements.InsideBounds(dataElementIndex)
               && Elements[dataElementIndex].InsideBounds(compositeElementIndex);
    }

    public bool CompositeElementNotNullOrEmpty(int dataElementIndex, int compositeElementIndex = 0)
    {
        return Elements.InsideBounds(dataElementIndex)
               && Elements[dataElementIndex].InsideBounds(compositeElementIndex)
               && Elements[dataElementIndex][compositeElementIndex].Length > 0;
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

    public void Assign(Segment other, Separators? separators = null, ILoop? parent = null)
    {
        Elements = other.Elements.Select(e => new Element(e)).ToList();

        if (separators is not null)
            Separators = separators;
    }

    public Segment Copy(Separators? separators = null, ILoop? parent = null)
    {
        var elements = Elements.Select(e => new Element(e)).ToList();
        return new Segment(elements, separators ?? Separators);
    }
}