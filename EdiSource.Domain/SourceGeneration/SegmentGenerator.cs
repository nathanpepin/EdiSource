using EdiSource.Domain.Loop;

namespace EdiSource.Domain.SourceGeneration;

/// <summary>
/// Mark a segment class with this attribute to enable source generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
// ReSharper disable once UnusedTypeParameter
#pragma warning disable CS9113 // Parameter is unread.
public sealed class SegmentGenerator<TParent>(string primaryId, string? secondaryId) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
    where TParent : ILoop;