namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EmptyAttribute(int dataElement, int compositeElement)
    : Attribute;