namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("INS")]
public sealed partial class _270_5010_Loop2100D_INS_DependentRelationship
{
    public bool? MemberIndicator
    {
        get => this.GetBool(1, "Y", falseValue: "N");
        set => this.SetBool(value, "Y", "N", 1);
    }

    public string? IndividualRelationshipCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? MaintenanceTypeCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}