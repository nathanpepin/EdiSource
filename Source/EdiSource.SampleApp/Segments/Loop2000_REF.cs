namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2000>("REF")]
public partial class Loop2000_REF
{
    public string ReferenceQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ReferenceId
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}