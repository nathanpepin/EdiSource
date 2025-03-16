using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Standard.Segments.STData;

namespace EdiSource.Domain.Structure.GenericTransactionSetData;

public sealed class GenericST : ST<GenericTransactionSet>, ISegmentIdentifier<GenericST>
{
    public override GenericTransactionSet? Parent { get; set; }
    public static EdiId EdiId { get; } = new("ST");
}