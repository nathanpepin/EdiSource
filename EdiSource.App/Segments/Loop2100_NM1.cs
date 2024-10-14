using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<Loop2100>("NM1", null)]
public partial class Loop2100_NM1 : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "WTF");
    }
}