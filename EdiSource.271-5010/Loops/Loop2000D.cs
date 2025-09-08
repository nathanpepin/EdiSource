namespace EdiSource._271_5010.Loops;

[LoopGenerator<_271, Loop2000D, Loop2000D_HL>]
public partial class Loop2000D : IValidatable
{
    [SegmentHeader] public Loop2000D_HL HL { get; set; } = default!;

    [Segment] public Loop2000D_TRN? TRN { get; set; }

    [Loop] public Loop2100D Loop2100D { get; set; } = default!;

    [LoopList] public LoopList<Loop2110D> Loop2110Ds { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Dependent Level
        if (HL.HierarchicalLevelCode != "23")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Dependent Level Code must be '23', got: {HL.HierarchicalLevelCode}");

        if (Loop2100D == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Dependent Name Loop (2100D) is required");

        if (Loop2110Ds.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "At least one Dependent Eligibility/Benefit Loop (2110D) is recommended");
    }
}
