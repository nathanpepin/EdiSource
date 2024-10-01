namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2000>("DTP")]
[BeDate(ValidationSeverity.Critical, 3, 0)]
public partial class Loop2000_DTP
{
    public string DateQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DateFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public DateTime Date
    {
        get => this.GetDateRequired(3);
        set => this.SetDate(value, 3);
    }
}