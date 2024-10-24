using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Standard.Loops;

public interface ITransactionSet
{
    static abstract TransactionSetDefinition Definition { get; }
    int GetTransactionSetSegmentCount();

    string GetTransactionSetControlNumber();
}