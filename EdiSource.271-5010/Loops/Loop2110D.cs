namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000D, Loop2110D, Loop2110D_EB>]
public partial class Loop2110D : IValidatable
{
    [SegmentHeader] public Loop2110D_EB EB { get; set; } = null!;

    [Segment] public Loop2110D_HSD? HSD { get; set; }

    [SegmentList] public SegmentList<Loop2110D_REF> REFs { get; set; } = [];

    [SegmentList] public SegmentList<Loop2110D_DTP> DTPs { get; set; } = [];

    [Segment] public Loop2110D_AAA? AAA { get; set; }

    [SegmentList] public SegmentList<Loop2110D_MSG> MSGs { get; set; } = [];

    [LoopList] public LoopList<Loop2115D> Loop2115Ds { get; set; } = [];

    [LoopList] public LoopList<Loop2120DGrouping> Loop2120Ds { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Eligibility/Benefit Information
        if (EB == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Eligibility/Benefit Information (EB) is required");

        // Validate eligibility/benefit indicator
        var validIndicators = new[]
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y"
        };
        if (!string.IsNullOrEmpty(EB?.EligibilityOrBenefitInformation) && !validIndicators.Contains(EB.EligibilityOrBenefitInformation))
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Warning,
                $"Unusual eligibility/benefit indicator: {EB.EligibilityOrBenefitInformation}");
    }
}