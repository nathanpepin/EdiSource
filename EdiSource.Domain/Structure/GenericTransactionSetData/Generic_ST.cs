using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Structure.GenericTransactionSetData;

public sealed class Generic_ST : ST<GenericTransactionSet>, ISegmentIdentifier<Generic_ST>
{
    public override GenericTransactionSet? Parent { get; set; }
    public static EdiId EdiId { get; } = new("ST");
}