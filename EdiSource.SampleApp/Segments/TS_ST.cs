using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Segments;

[SegmentGenerator<Loops._834>("ST", "834")]
public partial class TS_ST : IValidatable
{
    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (string.IsNullOrWhiteSpace(TransactionSetControlNumber))
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Transaction Set Control Number is required");
    }
}