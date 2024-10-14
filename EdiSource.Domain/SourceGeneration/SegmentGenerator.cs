using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;
#pragma warning disable CS9113 // Parameter is unread.

/// <summary>
///     Mark a segment class with this attribute to enable source generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SegmentGenerator<TParent>(string primaryId, string? secondaryId) : Attribute
    where TParent : ILoop;

public sealed class SegmentGenerator<TParent, TBase>(string primaryId, string? secondaryId) : Attribute
    where TParent : ILoop
    where TBase : Segment;
