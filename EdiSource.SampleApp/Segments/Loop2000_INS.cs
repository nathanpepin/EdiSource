using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Segments;

[SegmentGenerator<Loops.Loop2000>("INS")]
[IsOneOfValues(ValidationSeverity.Critical, 1, 0, "Y", "N")]
public partial class Loop2000_INS
{
    public string InsuredIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string IndividualRelationshipCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}