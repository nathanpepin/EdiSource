namespace EdiSource.SampleApp.Loops;

[LoopGenerator<_834, Loop2100, Loop2100_NM1>]
public partial class Loop2100 : IValidatable
{
    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; } = default!;

    [SegmentList] public SegmentList<Loop2100_DMG> Demographics { get; set; } = [];

    [SegmentList] public SegmentList<Loop2100_N3> Addresses { get; set; } = [];

    [SegmentList] public SegmentList<Loop2100_N4> CityStateZips { get; set; } = [];

    [SegmentList] public SegmentList<Loop2100_PER> ContactInfo { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate member details
        if (string.IsNullOrWhiteSpace(NM1.LastName))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Member last name is required");

        if (Demographics.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "Demographic information is recommended");
    }
}