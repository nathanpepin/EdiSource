namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000C, Loop2100C, Loop2100C_NM1>]
public partial class Loop2100C : IValidatable
{
    [SegmentHeader] public Loop2100C_NM1 NM1 { get; set; } = default!;

    [SegmentList] public SegmentList<Loop2100C_REF> REFs { get; set; } = [];

    [Segment] public Loop2100C_N3? N3 { get; set; }

    [Segment] public Loop2100C_N4? N4 { get; set; }

    [Segment] public Loop2100C_AAA? AAA { get; set; }

    [Segment] public Loop2100C_PRV? PRV { get; set; }

    [Segment] public Loop2100C_DMG? DMG { get; set; }

    [Segment] public Loop2100C_INS? INS { get; set; }

    [SegmentList] public SegmentList<Loop2100C_HI> HIs { get; set; } = [];

    [SegmentList] public SegmentList<Loop2100C_DTP> DTPs { get; set; } = [];

    [Segment] public Loop2100C_MPI? MPI { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Subscriber Name
        if (NM1.EntityIdentifierCode != "IL")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Subscriber Entity Identifier must be 'IL' (Insured), got: {NM1.EntityIdentifierCode}");

        if (string.IsNullOrWhiteSpace(NM1.SubscriberLastName) && string.IsNullOrWhiteSpace(NM1.EntityIdentifierCode))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Subscriber name is required");
    }
}
