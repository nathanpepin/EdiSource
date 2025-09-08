namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2120CGrouping, Loop2120C, Loop2120C_NM1>]
public partial class Loop2120C : IValidatable
{
    [SegmentHeader] public Loop2120C_NM1 NM1 { get; set; } = null!;

    [Segment] public Loop2120C_N3? N3 { get; set; }

    [Segment] public Loop2120C_N4? N4 { get; set; }

    [Segment] public Loop2120C_PER? PER { get; set; }

    [Segment] public Loop2120C_PRV? PRV { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        return [];
    }
}