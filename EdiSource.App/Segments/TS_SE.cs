using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_SE : SE<_834>, ISegmentIdentifier<TS_SE>
{
    public override _834? Parent { get; set; }
    public static EdiId EdiId { get; } = new("SE");
}