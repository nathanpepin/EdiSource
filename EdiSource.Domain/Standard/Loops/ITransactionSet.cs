using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Standard.Loops;

public interface ITransactionSet
    : ILoop
{
    ST ST { get; }
    SE SE { get; }
    static abstract TransactionSetDefinition Definition { get; }
}

public interface ITransactionSet<TSelf, TST, TSE> :
    ITransactionSet,
    IEdi<FunctionalGroup>, ISegmentIdentifier<TST>,
    ILoopInitialize<FunctionalGroup, TSelf>
    where TST : ST, ISegmentIdentifier<TST>
    where TSE : SE
    where TSelf : ILoop
{
    new TST ST { get; }
    new TSE SE { get; }
}