using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, Loop2100_NM1, Loop2100>]
public partial class Loop2100
{
    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; }
}