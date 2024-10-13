namespace EdiSource.Domain.Exceptions;

public sealed class DataElementParsingError(
    int dataElement,
    int compositeElement,
    Type type) : Exception($"Element {dataElement}:{compositeElement} is required to parse {type.Name}");