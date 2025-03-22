using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Segments;

[SegmentGenerator<Loops.Loop2100>("DMG")]
[BeDate(ValidationSeverity.Error, 2, 0)]
public partial class Loop2100_DMG
{
    public DateTime DateOfBirth
    {
        get => SegmentExtensions.GetDateRequired(this, 2);
        set => this.SetDate(value, 2);
    }

    public string Gender
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}