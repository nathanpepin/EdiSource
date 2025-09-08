namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2110C, Loop2120CGrouping, Loop2120C_LS>]
public partial class Loop2120CGrouping
{
    [SegmentHeader] public Loop2120C_LS? LS { get; set; }

    [LoopList] public LoopList<Loop2120C> Loop2120Cs { get; set; } = [];

    [SegmentFooter] public Loop2120C_LE? LE { get; set; }
}