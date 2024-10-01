using EdiSource._271_5010.Loop1000A_InformationSourceName.Segments;
using EdiSource._271_5010.TransactionSet;

namespace EdiSource._271_5010.Loop1000A_InformationSourceName;

[LoopGenerator<_271_5010_EligibilityBenefitResponse, _271_5010_Loop1000A_InformationSourceName, _271_5010_Loop1000A_NM1_InformationSourceName>]
public sealed partial class _271_5010_Loop1000A_InformationSourceName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop1000A_NM1_InformationSourceName NM1_InformationSourceName { get; set; } = null!;

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationSourceName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}