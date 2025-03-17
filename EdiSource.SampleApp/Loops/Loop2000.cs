using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Loops;

[LoopGenerator<_834, Loop2000, Segments.Loop2000_INS>]
public partial class Loop2000 : IValidatable
{
    [SegmentHeader] public Segments.Loop2000_INS INS { get; set; } = default!;

    [SegmentList] public SegmentList<Segments.Loop2000_REF> REFs { get; set; } = [];

    [SegmentList] public SegmentList<Segments.Loop2000_DTP> DTPs { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        // Example validation - ensure valid relationship code
        if (INS.IndividualRelationshipCode != "18" &&
            INS.IndividualRelationshipCode != "01" &&
            INS.IndividualRelationshipCode != "19")
        {
            yield return ValidationFactory.Create(
                (ILoop)this,
                ValidationSeverity.Warning,
                $"Unusual individual relationship code: {INS.IndividualRelationshipCode}");
        }
    }
}