using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<FunctionalGroup, _834, TS_ST>(true)]
public sealed partial class _834
{
    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP? DTP { get; set; } 

    [Loop] public Loop2000 Loop2000 { get; set; }  = default!;

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; } = default!;
}