using EdiSource._271_5010.Segments;

namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2120DGrouping, Loop2120D, Loop2120D_NM1>]
public partial class Loop2120D : IValidatable
{
    [SegmentHeader] public Loop2120D_NM1 NM1 { get; set; } = null!;

    [Segment] public Loop2120D_N3? N3 { get; set; }

    [Segment] public Loop2120D_N4? N4 { get; set; }

    [Segment] public Loop2120D_PER? PER { get; set; }

    [Segment] public Loop2120D_PRV? PRV { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        return [];
    }
}