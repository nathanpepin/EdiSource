namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000B, Loop2100B, Loop2100B_NM1>]
public partial class Loop2100B : IValidatable
{
    [SegmentHeader] public Loop2100B_NM1 NM1 { get; set; } = default!;

    [SegmentList] public SegmentList<Loop2100B_REF> REFs { get; set; } = [];

    [Segment] public Loop2100B_N3? N3 { get; set; }

    [Segment] public Loop2100B_N4? N4 { get; set; }

    [Segment] public Loop2100B_AAA? AAA { get; set; }

    [Segment] public Loop2100B_PRV? PRV { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Information Receiver Name
        if (NM1.EntityIdentifierCode != "1P")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Information Receiver Entity Identifier must be '1P' (Provider), got: {NM1.EntityIdentifierCode}");

        if (string.IsNullOrWhiteSpace(NM1.EntityIdentifierCode))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Information Receiver name is required");
    }
}