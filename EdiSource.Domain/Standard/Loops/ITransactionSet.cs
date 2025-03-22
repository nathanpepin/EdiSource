using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Standard.Loops;

public interface ITransactionSet<TSelf> : ILoop
    where TSelf : ITransactionSet<TSelf>,
    IEdi<FunctionalGroup>,
    ILoopInitialize<FunctionalGroup, TSelf>,
    ISegmentIdentifier<TSelf>
{
    static TransactionSetDefinition Definition =>
        TransactionSetDefinitionsFactory<TSelf>.CreateDefinition();

    public int GetTransactionSetSegmentCount()
    {
        return this.CountSegments();
    }

    public string GetTransactionSetControlNumber()
    {
        var segment = EdiItems.FirstOrDefault(x => x is ST<TSelf>) as ST<TSelf>;

        if (segment is null)
            throw new NotSupportedException("Must have ST segment");

        return segment.TransactionSetControlNumber;
    }
}