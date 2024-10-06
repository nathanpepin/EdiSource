namespace EdiSource.Domain.SourceGeneration;

/// <summary>
/// With a loop with the LoopGenerator attribute,
/// mark a segment with this attribute to enable source generation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class SegmentAttribute : Attribute;