namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2110D, Loop2115D, Loop2115D_III>]
public partial class Loop2115D : IValidatable
{
    [SegmentHeader] public Loop2115D_III III { get; set; } = default!;

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Additional Information
        if (III == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Additional Information (III) is required");
    }
}
