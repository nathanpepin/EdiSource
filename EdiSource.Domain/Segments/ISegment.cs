using EdiSource.Domain.Elements;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments;

public interface ISegment : IEdi
{
    IList<Element> Elements { get; set; }
    Separators Separators { get; }
    Element GetElement(int elementIndex);
    Element? GetElementOrNull(int elementIndex);
    string GetCompositeElement(int dataElementIndex, int compositeElementIndex);
    string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex);

    string GetDataElement(int dataElementIndex)
    {
        return GetCompositeElement(dataElementIndex, 0);
    }

    string? GetDataElementOrNull(int dataElementIndex)
    {
        return GetCompositeElementOrNull(dataElementIndex, 0);
    }

    bool SetDataElement(int elementIndex, params string[] values);
    bool SetCompositeElement(int dataElementIndex, int compositeElementIndex, string value);
    bool ElementExists(int elementIndex);
    bool CompositeElementExists(int dataElementIndex, int compositeElementIndex);
}

public interface ISegment<out TLoop>
    : ISegment
    where TLoop : ILoop
{
    TLoop? Parent { get; }
}