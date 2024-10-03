using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LoopGeneratorAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LoopGeneratorAttribute<TParent, TId> : Attribute
    where TParent : ILoop
    where TId : ISegment, ISegment<TParent>, ISegmentIdentifier<TId>, IEdi;
    
    /*
    
    [LoopGenerator<TransactionSet, TS_ST>]
    
    Add class implementation
        : ILoop<TransactionSet>, ISegmentIdentifier<TS_ST>,
        ISegmentIdentifier<TransactionSet>
    
    
    Add parent
        ILoop? ILoop.Parent => Parent;
        public TransactionSet? Parent => null;
        
    Add static ID
            public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
    */