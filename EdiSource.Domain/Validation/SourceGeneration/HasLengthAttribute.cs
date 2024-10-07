namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HasLengthAttribute(int dataElement, int compositeElement, int length)
    : Attribute;