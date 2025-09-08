namespace EdiSource.SampleApp.Loops;

[LoopGenerator<_834, Loop2000, Loop2000_INS>]
public partial class Loop2000 : IValidatable
{
    [SegmentHeader] public Loop2000_INS INS { get; set; } = default!;

    [SegmentList] public SegmentList<Loop2000_REF> REFs { get; set; } = [];

    [SegmentList] public SegmentList<Loop2000_DTP> DTPs { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Example validation - ensure valid relationship code
        if (INS.IndividualRelationshipCode != "18" &&
            INS.IndividualRelationshipCode != "01" &&
            INS.IndividualRelationshipCode != "19")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Warning,
                $"Unusual individual relationship code: {INS.IndividualRelationshipCode}");
    }
}