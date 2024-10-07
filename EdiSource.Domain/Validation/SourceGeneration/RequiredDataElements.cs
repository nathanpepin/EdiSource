namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RequiredDataElements(int[] dataElements)
    : Attribute;