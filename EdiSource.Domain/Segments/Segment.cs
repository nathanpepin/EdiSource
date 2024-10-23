using EdiSource.Domain.Elements;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments;

/// <summary>
///     A basic implementation of Segment
/// </summary>
public partial class Segment
{
    private Separators? _separators;

    public Segment(Segment segment, ILoop? parent = null)
    {
        Elements = segment.Elements;
        Separators = segment.Separators;
        // Parent = parent;
    }

    public Segment(IEnumerable<Element>? elements = null, Separators? separators = default, ILoop? parent = null)
    {
        Elements = elements?.ToList() ?? [];
        Separators = separators ?? Separators.DefaultSeparators;
        // Parent = parent;
    }

    public Segment(string segmentText, Separators? separators = null, ILoop? parent = null)
    {
        Elements = ReadElements(segmentText, separators).ToList();
        Separators = separators ?? Separators.DefaultSeparators;
        // Parent = parent;
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

    // public ILoop? Parent { get; set; }

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

    public void Assign(Segment other, Separators? separators = null, ILoop? parent = null)
    {
        Elements = other.Elements.Select(e => new Element(e)).ToList();

        if (separators is not null)
            Separators = separators;

        // if (parent is not null)
        // Parent = parent;
    }

    public Segment Copy(Separators? separators = null, ILoop? parent = null)
    {
        var elements = Elements.Select(e => new Element(e)).ToList();
        return new Segment(elements, separators ?? Separators); //, parent ?? Parent);
    }
}

public partial class Segment
{
    public override bool Equals(object? obj)
    {
        if (obj is not Segment segment) return false;
        return Equals(segment);
    }

    public bool Equals(Segment other)
    {
        if (other.Elements.Count != Elements.Count) return false;
        
        foreach (var (e, x) in other.Elements.Zip(Elements, (s, s1) => (s, s1)))
            if (!e.Equals(x))
                return false;

        return true;
    }

    public static bool operator ==(Segment left, Segment right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Segment element1, Segment element2)
    {
        return !(element1 == element2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this);
    }
}