namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2110C, Loop2115C, Loop2115C_III>]
public partial class Loop2115C : IValidatable
{
    [SegmentHeader] public Loop2115C_III III { get; set; } = default!;

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Additional Information
        if (III == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Additional Information (III) is required");
    }
}
