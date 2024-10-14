using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;

/// <summary>
///     Enables source generation to occur on a loop.
///     Generics are needed for the source generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class LoopGeneratorAttribute<TParent, TSelf, TId>(bool isTransactionSet = false) : Attribute
    where TParent : ILoop
    where TSelf : ILoop
    where TId : ISegment, ISegment<TSelf>, ISegmentIdentifier<TId>, IEdi;