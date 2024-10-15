using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Basic.Segments.REFData;

[IsOneOfValues(ValidationSeverity.Critical, 0, 0, "REF")]
[ElementLength(ValidationSeverity.Critical, 1, 80)]
[ElementLength(ValidationSeverity.Critical, 2, 80)]
public partial class REF : Segment, IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        if (Elements.Count < 2 ||
            (CompositeElementNotNullOrEmpty(1, 0) &&
             CompositeElementNotNullOrEmpty(2, 0)))
        {
            yield return ValidationFactory.CreateError(this, "REF required either 2 or 3 elements");
        }
    }
}