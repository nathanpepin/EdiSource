namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000A, Loop2100A, Loop2100A_NM1>]
public partial class Loop2100A : IValidatable
{
    [SegmentHeader] public Loop2100A_NM1 NM1 { get; set; } = null!;

    [Segment] public Loop2100A_PER? PER { get; set; }

    [Segment] public Loop2100A_AAA? AAA { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Information Source Name
        if (NM1.EntityIdentifierCode != "PR")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Information Source Entity Identifier must be 'PR' (Payer), got: {NM1.EntityIdentifierCode}");

        if (string.IsNullOrWhiteSpace(NM1.EntityIdentifierCode))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Information Source name is required");
    }
}