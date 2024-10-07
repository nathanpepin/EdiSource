namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ElementLengthAttribute(int dataElement, int compositeElement, int min, int max)
    : Attribute;