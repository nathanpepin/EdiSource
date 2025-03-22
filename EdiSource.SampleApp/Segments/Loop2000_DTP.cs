using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Segments;

[SegmentGenerator<Loops.Loop2000>("DTP")]
[BeDate(ValidationSeverity.Critical, 3, 0)]
public partial class Loop2000_DTP
{
    public string DateQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DateFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public DateTime Date
    {
        get => SegmentExtensions.GetDateRequired(this, 3);
        set => this.SetDate(value, 3);
    }
}