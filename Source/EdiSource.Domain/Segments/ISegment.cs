namespace EdiSource.Domain.Segments;

/// <summary>
///     Defines the contract for an EDI segment, providing indexed access to data elements
///     and composite elements, along with mutation and query operations.
/// </summary>
public interface ISegment
{
    /// <summary>Gets or sets the first composite element at the given data element index.</summary>
    string this[int index] { get; set; }

    /// <summary>Gets or sets a specific composite element.</summary>
    string this[int dataElement, int compositeElement] { get; set; }

    /// <summary>The list of data elements in this segment.</summary>
    IList<Element> Elements { get; set; }

    /// <summary>The separator configuration used by this segment.</summary>
    Separators Separators { get; set; }

    /// <summary>Gets the data element at the specified index. Throws if out of bounds.</summary>
    Element GetElement(int elementIndex);

    /// <summary>Gets the data element at the specified index, or null if out of bounds.</summary>
    Element? GetElementOrNull(int elementIndex);

    /// <summary>Gets the composite element value at the specified indices. Throws if not found.</summary>
    string GetCompositeElement(int dataElementIndex, int compositeElementIndex = 0);

    /// <summary>Gets the composite element value at the specified indices, or null if not found.</summary>
    string? GetCompositeElementOrNull(int dataElementIndex, int compositeElementIndex = 0);

    /// <summary>Sets a data element's values, optionally creating it if it doesn't exist.</summary>
    bool SetDataElement(int elementIndex, bool create = true, params string[] values);

    /// <summary>Sets a composite element's value, optionally creating it if it doesn't exist.</summary>
    bool SetCompositeElement(string? value, int dataElementIndex, int compositeElementIndex = 0,
        bool create = true);

    /// <summary>Returns true if a data element exists at the specified index.</summary>
    bool ElementExists(int elementIndex);

    /// <summary>Returns true if a composite element exists at the specified indices.</summary>
    bool CompositeElementExists(int dataElementIndex, int compositeElementIndex = 0);

    /// <summary>Returns true if the composite element exists and is not null or empty.</summary>
    bool CompositeElementNotNullOrEmpty(int dataElementIndex, int compositeElementIndex = 0);

    /// <summary>Copies all elements from another segment into this one.</summary>
    void Assign(Segment other, Separators? separators = null);

    /// <summary>Creates a deep copy of this segment.</summary>
    Segment Copy(Separators? separators = null);
}