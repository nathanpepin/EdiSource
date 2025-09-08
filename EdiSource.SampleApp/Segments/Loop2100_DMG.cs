namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2100>("DMG")]
[BeDate(ValidationSeverity.Error, 2, 0)]
public partial class Loop2100_DMG
{
    public DateTime DateOfBirth
    {
        get => this.GetDateRequired(2);
        set => this.SetDate(value, 2);
    }

    public string Gender
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}