namespace EdiSource._271_5010.Loops;

[LoopGenerator<_271, Loop2000B, Loop2000B_HL>]
public partial class Loop2000B : IValidatable
{
    [SegmentHeader] public Loop2000B_HL HL { get; set; } = null!;

    [Loop] public Loop2100B Loop2100B { get; set; } = null!;

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Information Receiver Level
        if (HL.HierarchicalLevelCode != "21")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Information Receiver Level Code must be '21', got: {HL.HierarchicalLevelCode}");

        if (Loop2100B == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Information Receiver Name Loop (2100B) is required");
    }
}
