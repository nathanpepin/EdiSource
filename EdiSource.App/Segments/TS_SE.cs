using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<TransactionSet>("SE", null)]
public partial class TS_SE : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "No");
    }
}