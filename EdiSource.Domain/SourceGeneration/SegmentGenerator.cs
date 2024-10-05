using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SegmentGenerator<TParent>(string primaryId, string? secondaryId) : Attribute
    where TParent : ILoop;
