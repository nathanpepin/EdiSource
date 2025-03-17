using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Segments;

[SegmentGenerator<Loops._834>("REF")]
[ElementLength(ValidationSeverity.Error, 1, 3)]
public partial class TS_REF
{
    public string ReferenceQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ReferenceId
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}