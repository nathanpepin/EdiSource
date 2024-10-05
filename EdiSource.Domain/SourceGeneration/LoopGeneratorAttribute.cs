using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LoopGeneratorAttribute<TParent, TSelf, TId> : Attribute
    where TParent : ILoop
    where TSelf : ILoop
    where TId : ISegment, ISegment<TSelf>, ISegmentIdentifier<TId>, IEdi;