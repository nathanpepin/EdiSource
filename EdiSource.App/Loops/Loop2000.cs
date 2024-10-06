using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, Loop2000, Loop2000_INS>]
public partial class Loop2000 : IValidatable
{
    [SegmentHeader] public Loop2000_INS INS { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "Nof");
    }

    
}