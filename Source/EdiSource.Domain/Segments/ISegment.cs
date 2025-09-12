namespace EdiSource.Domain.Segments;

public interface ISegment
{
    string this[int index] { get; set; }
    string this[int dataElement, int compositeElement] { get; set; }
    IList<Element> Elements { get; set; }
    Separators Separators { get; set; }
    Element GetElement(int elementIndex);
    Element? GetElementOrNull(int elementIndex);
    string GetCompositeElement(int dataElementIndex, int compositeElementIndex = 0);
    string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex = 0);
    bool SetDataElement(int elementIndex, bool create = true, params string[] values);

    bool SetCompositeElement(string? value, int dataElementIndex, int compositeElementIndex = 0,
        bool create = true);

    bool ElementExists(int elementIndex);
    bool CompositeElementExists(int dataElementIndex, int compositeElementIndex = 0);
    bool CompositeElementNotNullOrEmpty(int dataElementIndex, int compositeElementIndex = 0);
    void Assign(Segment other, Separators? separators = null, ILoop? parent = null);
    Segment Copy(Separators? separators = null, ILoop? parent = null);
}