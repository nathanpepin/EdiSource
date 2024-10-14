using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;

namespace EdiSource.Domain.Standard.Loops;

public interface ITransactionSet : ILoop
{
    ISegment ST { get; }

    ISegment SE { get; }

    // static abstract TransactionSetDefinition Definition { get; }
}

public interface ITransactionSet<TSelf, TId> :
    ITransactionSet,
    ILoop<FunctionalGroup>, ISegmentIdentifier<TId>,
    ILoopInitialize<FunctionalGroup, TSelf>
    where TSelf : ITransactionSet<TSelf, TId>
    where TId : ISegmentIdentifier<TId>
{
    new static abstract TransactionSetDefinition<TSelf> Definition { get; }
}