using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Standard.Segments.STData;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_ST : ST<_834>, ISegmentIdentifier<TS_ST>
{
    public override _834? Parent { get; set; }

    public static EdiId EdiId { get; } = new("ST", "834");
}