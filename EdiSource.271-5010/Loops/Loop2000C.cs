namespace EdiSource._271_5010.Loops;

[LoopGenerator<_271, Loop2000C, Loop2000C_HL>]
public partial class Loop2000C : IValidatable
{
    [SegmentHeader] public Loop2000C_HL HL { get; set; } = default!;

    [Segment] public Loop2000C_TRN? TRN { get; set; }

    [Loop] public Loop2100C Loop2100C { get; set; } = default!;

    [LoopList] public LoopList<Loop2110C> Loop2110Cs { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Subscriber Level
        if (HL.HierarchicalLevelCode != "22")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Subscriber Level Code must be '22', got: {HL.HierarchicalLevelCode}");

        if (Loop2100C == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Subscriber Name Loop (2100C) is required");

        if (Loop2110Cs.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "At least one Eligibility/Benefit Loop (2110C) is recommended");
    }
}
