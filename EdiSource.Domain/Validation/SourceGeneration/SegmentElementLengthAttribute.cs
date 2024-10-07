namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SegmentElementLengthAttribute(int min, int max)
    : Attribute;