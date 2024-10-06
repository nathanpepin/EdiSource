namespace EdiSource.Domain.SourceGeneration;

/// <summary>
/// With a loop with the LoopGenerator attribute,
/// mark a loop list with this attribute to enable source generation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class LoopListAttribute : Attribute;