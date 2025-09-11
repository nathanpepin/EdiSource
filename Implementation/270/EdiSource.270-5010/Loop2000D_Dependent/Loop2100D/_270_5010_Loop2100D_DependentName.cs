using EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Loop2110D;
using EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D;

[LoopGenerator<_270_5010_Loop2000D_Dependent, _270_5010_Loop2100D_DependentName, _270_5010_Loop2100D_NM1_DependentName>]
public sealed partial class _270_5010_Loop2100D_DependentName : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2100D_NM1_DependentName NM1_DependentName { get; set; } = null!;

    [SegmentList] public SegmentList<_270_5010_Loop2100D_REF_DependentAdditionalID> REF_DependentAdditionalIDs { get; set; } = [];

    [Segment] public _270_5010_Loop2100D_N3_DependentAddress? N3_DependentAddress { get; set; }

    [Segment] public _270_5010_Loop2100D_N4_DependentCityStateZIP? N4_DependentCityStateZIP { get; set; }

    [Segment] public _270_5010_Loop2100D_PRV_ProviderInfo? PRV_ProviderInfo { get; set; }

    [Segment] public _270_5010_Loop2100D_DMG_DependentDemographics? DMG_DependentDemographics { get; set; }

    [Segment] public _270_5010_Loop2100D_INS_DependentRelationship? INS_DependentRelationship { get; set; }

    [Segment] public _270_5010_Loop2100D_HI_DependentHealthCareDiagnosisCode? HI_DependentHealthCareDiagnosisCode { get; set; }

    [SegmentList] public SegmentList<_270_5010_Loop2100D_DTP_DependentDate> DTP_DependentDates { get; set; } = [];

    [LoopList] public LoopList<_270_5010_Loop2110D_DependentEligibilityBenefitInquiry> Loop2110D_DependentEligibilityBenefitInquiries { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
#pragma warning disable CS8602
        if (NM1_DependentName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
#pragma warning restore CS8602

        if (!Loop2110D_DependentEligibilityBenefitInquiries.Any())
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "At least one Dependent Eligibility/Benefit Inquiry loop (2110D) is required");

        if (N3_DependentAddress != null && N4_DependentCityStateZIP == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning,
                "N4 segment is recommended when N3 segment is present");
    }
}