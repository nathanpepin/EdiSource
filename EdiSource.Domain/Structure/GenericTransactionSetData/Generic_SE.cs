using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Standard.Segments;

namespace EdiSource.Domain.Structure.GenericTransactionSetData;

public sealed class Generic_SE : SE<GenericTransactionSet>, ISegmentIdentifier<Generic_SE>
{
    public override GenericTransactionSet? Parent { get; set; }
    public static EdiId EdiId { get; } = new("SE");
}