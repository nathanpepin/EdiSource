using EdiSource._270_5010.Loop2000D_Dependent.Loop2100D;
using EdiSource._270_5010.Loop2000D_Dependent.Segments;

namespace EdiSource._270_5010.Loop2000D_Dependent;

[LoopGenerator<_270_5010_EligibilityBenefitInquiry, _270_5010_Loop2000D_Dependent, _270_5010_Loop2000D_HL_Dependent>]
public sealed partial class _270_5010_Loop2000D_Dependent : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2000D_HL_Dependent HL_Dependent { get; set; } = null!;

    [SegmentList] public SegmentList<_270_5010_Loop2000D_TRN_TraceNumber> TRN_TraceNumbers { get; set; } = [];

    [Loop] public _270_5010_Loop2100D_DependentName? Loop2100D_DependentName { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
#pragma warning disable CS8602
        if (HL_Dependent == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "HL Dependent segment is required");
#pragma warning restore CS8602

        if (Loop2100D_DependentName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Dependent Name loop (2100D) is required");

        if (TRN_TraceNumbers.Count > 2)
            yield return ValidationFactory.Create(this, ValidationSeverity.Error,
                "Maximum of 2 TRN Trace Number segments allowed");

        if (HL_Dependent?.HierarchicalLevelCode != "23")
            yield return ValidationFactory.Create(this, ValidationSeverity.Error,
                "Hierarchical Level Code must be '23' for dependent");
    }
}