namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("VEH")]
public sealed partial class _270_5010_Loop2110C_VEH_AdditionalInquiry
{
    public int? AssignedNumber
    {
        get => this.GetInt(1);
        set => this.SetInt(value, 1);
    }
    
    public string? VehicleIdentificationNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
    
    public int? Year
    {
        get => this.GetInt(3);
        set => this.SetInt(value, 3);
    }
    
    public string? AgencyQualifierCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
    
    public string? ProductDescriptionCode
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }
    
    public string? ProductDescriptionCode2
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }
    
    public string? VehiclePoolIdentifier
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
    
    public string? InventoryUseCode
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}