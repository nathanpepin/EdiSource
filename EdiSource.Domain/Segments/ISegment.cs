using EdiSource.Domain.Elements;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Segments;

/// <summary>
///     Represents an EDI (Electronic Data Interchange) segment. This interface encapsulates methods and properties
///     for handling EDI segments, their elements, composite elements, and associated separators.
/// </summary>
public interface ISegment : IEdi, IParent
{
    /// <summary>
    ///     Gets or sets the collection of elements associated with the segment.
    /// </summary>
    IList<Element> Elements { get; set; }

    /// <summary>
    ///     Gets the separators used to parse the EDI segments.
    ///     The separators include segment separator, data element separator, and composite element separator.
    /// </summary>
    Separators Separators { get; }

    /// <summary>
    ///     Retrieves the element at the specified index within the segment.
    /// </summary>
    /// <param name="elementIndex">The index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    Element GetElement(int elementIndex);

    /// <summary>
    ///     Retrieves the element at the specified index, or returns null if the index is out of range.
    /// </summary>
    /// <param name="elementIndex">The index of the element to retrieve.</param>
    /// <returns>The element at the specified index, or null if the index is out of range.</returns>
    Element? GetElementOrNull(int elementIndex);

    /// <summary>
    ///     Retrieves the value of a composite element at the specified data element index and composite element index.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element.</param>
    /// <param name="compositeElementIndex">The index of the composite element within the data element.</param>
    /// <returns>The value of the specified composite element.</returns>
    string GetCompositeElement(int dataElementIndex, int compositeElementIndex);

    /// <summary>
    ///     Retrieves the value of a composite element at the specified data element index and
    ///     composite element index, or returns null if the composite element does not exist.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element containing the composite element.</param>
    /// <param name="compositeElementIndex">The index of the composite element within the data element.</param>
    /// <returns>The value of the composite element if it exists; otherwise, null.</returns>
    string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex);

    /// <summary>
    ///     Retrieves a data element based on the specified data element index.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element to retrieve.</param>
    /// <returns>Returns the data element as a string.</returns>
    string GetDataElement(int dataElementIndex)
    {
        return GetCompositeElement(dataElementIndex, 0);
    }

    /// <summary>
    ///     Retrieves the data element at the specified index, or returns null if the data element does not exist.
    /// </summary>
    /// <param name="dataElementIndex">The zero-based index of the data element to retrieve.</param>
    /// <returns>The data element at the specified index if it exists; otherwise, null.</returns>
    string? GetDataElementOrNull(int dataElementIndex)
    {
        return GetCompositeElementOrNull(dataElementIndex, 0);
    }

    /// <summary>
    ///     Sets the data element at the specified index with the provided values.
    /// </summary>
    /// <param name="elementIndex">The index of the element to be set.</param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <param name="values">The values to set for the data element.</param>
    /// <returns>True if the element exists and the values are set, otherwise false.</returns>
    bool SetDataElement(int elementIndex, bool create = true, params string[] values);

    /// <summary>
    ///     Sets the value of a composite element within a data element at the specified indices.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element that contains the composite element.</param>
    /// <param name="compositeElementIndex">The index of the composite element within the data element to set.</param>
    /// <param name="value">The value to set for the specified composite element.</param>
    /// <param name="create">Will create the preceding composite data elements and composite elements if needed</param>
    /// <returns>True if the composite element was successfully set; otherwise, false.</returns>
    bool SetCompositeElement(int dataElementIndex, int compositeElementIndex, string value, bool create = true);

    /// <summary>
    ///     Checks if an element exists at the specified index.
    /// </summary>
    /// <param name="elementIndex">The index of the element to check for existence.</param>
    /// <returns>True if the element exists, otherwise false.</returns>
    bool ElementExists(int elementIndex);

    /// <summary>
    ///     Checks if a composite element exists at the specified data element and composite element indexes.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element.</param>
    /// <param name="compositeElementIndex">The index of the composite element within the data element.</param>
    /// <returns>True if the composite element exists; otherwise, false.</returns>
    bool CompositeElementExists(int dataElementIndex, int compositeElementIndex);

    /// <summary>
    ///     Checks if a composite element exists at the specified data element and composite element indexes
    /// and if it has a length greater than 0.
    /// </summary>
    /// <param name="dataElementIndex">The index of the data element.</param>
    /// <param name="compositeElementIndex">The index of the composite element within the data element.</param>
    /// <returns>True if the composite element exists; otherwise, false.</returns>
    bool CompositeElementNotNullOrEmpty(int dataElementIndex, int compositeElementIndex);
}

/// <summary>
///     Interface defining the structure and behavior of an EDI segment, including parent loop, elements, and separators.
///     Encapsulates methods for retrieving and manipulating both simple and composite elements.
/// </summary>
public interface ISegment<out TLoop>
    : ISegment
    where TLoop : ILoop
{
    /// <summary>
    ///     The typed parent loop of the segment if any
    /// </summary>
    new TLoop? Parent { get; }

    /// <summary>
    ///     Assigns data elements safety one segment to another.
    ///     Useful for avoiding the issue where two segments share
    ///     the same reference to a List.
    ///     The parent and separators will still be shared if not specified.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="separators">Will use segment values if not provided</param>
    /// <param name="parent">Will use segment values if not provided</param>
    void Assign(ISegment other, Separators? separators = null, ILoop? parent = null);

    /// <summary>
    ///     Copies data elements safety one segment to a new segment.
    ///     Useful for avoiding the issue where two segments share
    ///     the same reference to a List.
    ///     The parent and separators will still be shared if not specified.
    ///     <param name="separators">Will use segment values if not provided</param>
    ///     <param name="parent">Will use segment values if not provided</param>
    /// </summary>
    /// <returns></returns>
    ISegment Copy(Separators? separators = null, ILoop? parent = null);
}