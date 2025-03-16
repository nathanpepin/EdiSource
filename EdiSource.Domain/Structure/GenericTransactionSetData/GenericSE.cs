using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Standard.Segments;

namespace EdiSource.Domain.Structure.GenericTransactionSetData;

public sealed class GenericSE : SE<GenericTransactionSet>, ISegmentIdentifier<GenericSE>
{
    public override GenericTransactionSet? Parent { get; set; }
    public static EdiId EdiId { get; } = new("SE");
}