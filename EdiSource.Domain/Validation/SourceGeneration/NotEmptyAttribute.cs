namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class NotEmptyAttribute(int dataElement, int compositeElement)
    : Attribute;