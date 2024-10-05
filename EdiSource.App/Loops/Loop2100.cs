using EdiSource.Domain.Loop;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, Loop2100, Loop2100_NM1>]
public partial class Loop2100
{
    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; }
}