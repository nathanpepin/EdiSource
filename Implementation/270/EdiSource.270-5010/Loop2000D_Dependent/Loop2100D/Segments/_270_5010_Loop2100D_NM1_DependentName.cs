namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("NM1", "03")]
public sealed partial class _270_5010_Loop2100D_NM1_DependentName
{
    public string? EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? EntityTypeQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? LastOrOrganizationName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? FirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? MiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string? NamePrefix
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string? NameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string? IdentificationCode
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}