using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Loops;

[LoopGenerator<_834, Loop2100, Segments.Loop2100_NM1>]
public partial class Loop2100 : IValidatable
{
    [SegmentHeader] public Segments.Loop2100_NM1 NM1 { get; set; } = default!;

    [SegmentList] public SegmentList<Segments.Loop2100_DMG> Demographics { get; set; } = [];

    [SegmentList] public SegmentList<Segments.Loop2100_N3> Addresses { get; set; } = [];

    [SegmentList] public SegmentList<Segments.Loop2100_N4> CityStateZips { get; set; } = [];

    [SegmentList] public SegmentList<Segments.Loop2100_PER> ContactInfo { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate member details
        if (string.IsNullOrWhiteSpace(NM1.LastName))
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error, "Member last name is required");

        if (Demographics.Count == 0)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Warning, "Demographic information is recommended");
    }
}