namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2110D, Loop2120DGrouping, Loop2120D_LS>]
public partial class Loop2120DGrouping
{
    [SegmentHeader] public Loop2120D_LS? LS { get; set; }
    [LoopList] public LoopList<Loop2120D> Loop2120Ds { get; set; } = [];
    [SegmentFooter] public Loop2120D_LE? LE { get; set; }
}