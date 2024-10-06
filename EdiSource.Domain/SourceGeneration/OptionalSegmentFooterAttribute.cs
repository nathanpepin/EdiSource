namespace EdiSource.Domain.SourceGeneration;

/// <summary>
/// With a loop with the LoopGenerator attribute,
/// mark a segment footer with this attribute to enable source generation.
/// When the loop constructor finds this element, it will break out the loop.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionalSegmentFooterAttribute : Attribute;