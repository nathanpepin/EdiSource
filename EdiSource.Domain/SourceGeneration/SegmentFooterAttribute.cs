namespace EdiSource.Domain.SourceGeneration;

/// <summary>
/// With a loop with the LoopGenerator attribute,
/// mark a loop with this attribute to enable source generation.
/// A segment footer is required and will be expected at the end.
/// For instance, an IEA is the footer of and ISA.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class SegmentFooterAttribute : Attribute;