namespace EdiSource._271_5010.Loops;

[LoopGenerator<Loop2000D, Loop2100D, Loop2100D_NM1>]
public partial class Loop2100D : IValidatable
{
    [SegmentHeader] public Loop2100D_NM1 NM1 { get; set; } = null!;

    [SegmentList] public SegmentList<Loop2100D_REF> REFs { get; set; } = [];

    [Segment] public Loop2100D_N3? N3 { get; set; }

    [Segment] public Loop2100D_N4? N4 { get; set; }

    [Segment] public Loop2100D_AAA? AAA { get; set; }

    [Segment] public Loop2100D_PRV? PRV { get; set; }

    [Segment] public Loop2100D_DMG? DMG { get; set; }

    [Segment] public Loop2100D_INS? INS { get; set; }

    [SegmentList] public SegmentList<Loop2100D_HI> HIs { get; set; } = [];

    [SegmentList] public SegmentList<Loop2100D_DTP> DTPs { get; set; } = [];

    [Segment] public Loop2100D_MPI? MPI { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate Dependent Name
        if (NM1.EntityIdentifierCode != "03")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                $"Dependent Entity Identifier must be '03' (Dependent), got: {NM1.EntityIdentifierCode}");

        if (string.IsNullOrWhiteSpace(NM1.DependentLastName) && string.IsNullOrWhiteSpace(NM1.EntityIdentifierCode))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Dependent name is required");
    }
}
