namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2100>("N4")]
public partial class Loop2100_N4
{
    public string City
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string State
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string PostalCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}