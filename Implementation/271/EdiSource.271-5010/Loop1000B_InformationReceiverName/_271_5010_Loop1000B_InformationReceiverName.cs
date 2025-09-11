using EdiSource._271_5010.Loop1000B_InformationReceiverName.Segments;
using EdiSource._271_5010.TransactionSet;

namespace EdiSource._271_5010.Loop1000B_InformationReceiverName;

[LoopGenerator<_271_5010_EligibilityBenefitResponse, _271_5010_Loop1000B_InformationReceiverName, _271_5010_Loop1000B_NM1_InformationReceiverName>]
public sealed partial class _271_5010_Loop1000B_InformationReceiverName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop1000B_NM1_InformationReceiverName NM1_InformationReceiverName { get; set; } = null!;

    [Segment]  public _271_5010_Loop1000B_N3_InformationReceiverAddress? N3_InformationReceiverAddress { get; set; }
    [Segment] public _271_5010_Loop1000B_N4_InformationReceiverGeographicLocation? N4_InformationReceiverGeographicLocation { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationReceiverName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}
