using EdiSource.Basic.Segments.DTPData;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[ElementLength(ValidationSeverity.Critical, 0, 20)]
[SegmentGenerator<_834, DTP>("DTP", null)]
public partial class TS_DTP
{
    public TS_DTP()
    {
        List<IIndirectValidatable> k =
            [..base.SourceGenValidations, ..SourceGenValidations];
    }
}