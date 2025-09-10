namespace EdiSource._271_5010.Loop1000A_InformationSourceName.Segments;

[SegmentGenerator<_271_5010_Loop1000A_InformationSourceName>("NM1", $"2B{S}36{S}GP{S}P5{S}PR")]
public partial class _271_5010_Loop1000A_NM1_InformationSourceName
{
    public string? EntityIdentifierCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? EntityTypeQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? InformationSourceLastOrOrganizationName { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? InformationSourceFirstName { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
    public string? InformationSourceMiddleName { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public string? NamePrefix { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public string? NameSuffix { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
    public string? IdentificationCodeQualifier { get => GetCompositeElement(8); set => SetCompositeElement(value, 8); }
    public string? InformationSourceIdentifier { get => GetCompositeElement(9); set => SetCompositeElement(value, 9); }
}
