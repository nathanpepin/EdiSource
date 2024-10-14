using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;

#pragma warning disable CS9113 // Parameter is unread.

/// <summary>
///     Enables source generation to occur on a loop.
///     Generics are needed for the source generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class LoopGeneratorAttribute<TParent, TSelf, TId>(bool isTransactionSet = false) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
    where TParent : ILoop
    where TSelf : ILoop
    where TId : ISegment, ISegment<TSelf>, ISegmentIdentifier<TId>, IEdi;