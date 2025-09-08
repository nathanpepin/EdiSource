namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000C, Loop2110C, Loop2110C_EB>]
public partial class Loop2110C : IValidatable
{
    [SegmentHeader] public Loop2110C_EB EB { get; set; } = null!;

    [Segment] public Loop2110C_HSD? HSD { get; set; }

    [SegmentList] public SegmentList<Loop2110C_REF> REFs { get; set; } = [];

    [SegmentList] public SegmentList<Loop2110C_DTP> DTPs { get; set; } = [];

    [Segment] public Loop2110C_AAA? AAA { get; set; }

    [SegmentList] public SegmentList<Loop2110C_MSG> MSGs { get; set; } = [];

    [LoopList] public LoopList<Loop2115C> Loop2115Cs { get; set; } = [];

    [LoopList] public LoopList<Loop2120CGrouping> Loop2120Cs { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Eligibility/Benefit Information
        if (EB == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Eligibility/Benefit Information (EB) is required");

        // Validate eligibility/benefit indicator
        var validIndicators = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y" };
        if (!string.IsNullOrEmpty(EB.EligibilityOrBenefitInformation) && !validIndicators.Contains(EB.EligibilityOrBenefitInformation))
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Warning,
                $"Unusual eligibility/benefit indicator: {EB.EligibilityOrBenefitInformation}");
    }
}
