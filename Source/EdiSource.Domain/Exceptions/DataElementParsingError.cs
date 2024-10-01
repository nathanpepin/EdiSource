namespace EdiSource.Domain.Exceptions;

/// <summary>
///     Thrown when a required data element is missing or cannot be parsed into the expected type.
/// </summary>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
/// <param name="type">The target type that the element was expected to be parsed into.</param>
public sealed class DataElementParsingError(
    int dataElement,
    int compositeElement,
    Type type) : Exception($"Element {dataElement}:{compositeElement} is required to parse {type.Name}");