namespace EdiSource.Domain.SourceGeneration;

/// <summary>
///     With a loop with the LoopGenerator attribute,
///     mark a loop with this attribute to enable source generation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class LoopAttribute : Attribute;