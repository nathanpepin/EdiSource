using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<_834, SE>("SE", null)]
[ElementLength(0, 1, 3, 3)]
public partial class TS_SE : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "No");
    }
}