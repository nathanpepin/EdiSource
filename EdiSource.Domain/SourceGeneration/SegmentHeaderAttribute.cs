namespace EdiSource.Domain.SourceGeneration;

/// <summary>
///     With a loop with the LoopGenerator attribute,
///     mark a segment header with this attribute to enable source generation.
///     A segment header is required and is expected to begin a loop.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class SegmentHeaderAttribute : Attribute;