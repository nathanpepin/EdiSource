using EdiSource._271_5010.Segments;

namespace EdiSource._271_5010.Loops;

[LoopGenerator<_271, Loop2000A, Loop2000A_HL>]
public partial class Loop2000A : IValidatable
{
    [SegmentHeader] public Loop2000A_HL HL { get; set; } = null!;

    [Segment] public Loop2000A_AAA? AAA { get; set; }

    [Loop] public Loop2100A Loop2100A { get; set; } = null!;

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Information Source Level
        if (HL.HierarchicalLevelCode != "20")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Information Source Level Code must be '20', got: {HL.HierarchicalLevelCode}");

        if (Loop2100A == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Information Source Name Loop (2100A) is required");
    }
}