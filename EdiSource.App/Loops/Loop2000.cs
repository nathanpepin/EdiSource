using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, Loop2000, Loop2000_INS>]
public partial class Loop2000
{
    [SegmentHeader] public Loop2000_INS INS { get; set; }
}