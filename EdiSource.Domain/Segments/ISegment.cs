using System.Runtime.CompilerServices;
using System.Text;
using EdiSource.Domain.Elements;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Segments;

public interface ISegment : IEdi
{
    IList<Element> Elements { get; set; }
    Element GetElement(int elementIndex);
    Element? GetElementOrNull(int elementIndex);
    string GetCompositeElement(int dataElementIndex, int compositeElementIndex);
    string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex);

    string GetDataElement(int dataElementIndex) => GetCompositeElement(dataElementIndex, 0);

    string? GetDataElementOrNull(int dataElementIndex) => GetCompositeElementOrNull(dataElementIndex, 0);
    bool SetDataElement(int elementIndex, params string[] values);
    bool SetCompositeElement(int dataElementIndex, int compositeElementIndex, string value);
    bool ElementExists(int elementIndex);
    bool CompositeElementExists(int dataElementIndex, int compositeElementIndex);
    Separators SegmentSeparators { get; }
}

public interface ISegment<out TLoop>
    : ISegment
    where TLoop : ILoop
{
    new TLoop? Parent { get; }
}