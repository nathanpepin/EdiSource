using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SegmentElementLengthAttribute(int min, int max)
    : Attribute;