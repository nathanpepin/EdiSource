namespace EdiSource.Domain.Segments;

/// <summary>
///     A basic implementation of Segment
/// </summary>
public partial class Segment : IEdi, ISegment
{
    private static readonly ConcurrentDictionary<Type, Func<EdiId?>> EdiIdGetters = new();

    private readonly List<Element> _elements = [];
    private Separators? _separators;

    protected Segment()
    {
        Separators = Separators.DefaultSeparators;

        var type = GetType();
        if (EdiIdGetters.TryGetValue(type, out var getEdiId))
        {
            var ediId = getEdiId();
            ediId?.CopyIdElementsToSegment(this);
        }
        else
        {
            var segmentIdentifierInterface = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(ISegmentIdentifier<>));

            if (segmentIdentifierInterface != null)
            {
                var ediIdProperty = type.GetProperty("EdiId",
                    BindingFlags.Public | BindingFlags.Static);

                if (ediIdProperty != null)
                {
                    EdiId? Getter()
                    {
                        return ediIdProperty.GetValue(null) as EdiId?;
                    }

                    EdiIdGetters.TryAdd(type, Getter);

                    var ediId = Getter();
                    ediId?.CopyIdElementsToSegment(this);
                }
                else
                {
                    EdiIdGetters.TryAdd(type, () => null);
                }
            }
            else
            {
                EdiIdGetters.TryAdd(type, () => null);
            }
        }
    }

    public Segment(Segment segment, ILoop? parent = null)
    {
        Separators = segment.Separators;

        var copy = segment.Copy(Separators, parent);
        Elements = copy.Elements;
    }

    public Segment(IEnumerable<Element>? elements = null, Separators? separators = null, ILoop? parent = null)
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

    public IList<Element> Elements
    {
        get => _elements;
        set
        {
            _elements.Clear();
            _elements.AddRange(value);
        }
    }

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

    public bool SetCompositeElement(string? value, int dataElementIndex, int compositeElementIndex = 0,
        bool create = true)
    {
        if (create && !ElementExists(dataElementIndex))
            while (dataElementIndex >= Elements.Count)
                Elements.Add([]);
        else if (!ElementExists(dataElementIndex)) return false;

        if (create && !CompositeElementExists(dataElementIndex, compositeElementIndex))
            while (!CompositeElementExists(dataElementIndex, compositeElementIndex))
                Elements[dataElementIndex].Add(string.Empty);
        else if (!CompositeElementExists(dataElementIndex, compositeElementIndex)) return false;

        Elements[dataElementIndex][compositeElementIndex] = value ?? string.Empty;
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