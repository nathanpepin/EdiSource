namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class IsOneOfValuesAttribute(int dataElement, int compositeElement, params string[] values)
    : Attribute;