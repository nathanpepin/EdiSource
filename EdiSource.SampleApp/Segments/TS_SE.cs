using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Segments;

[SegmentGenerator<Loops._834>("SE")]
public partial class TS_SE : IValidatable
{
    public int SegmentCount
    {
        get => SegmentExtensions.GetIntRequired(this, 1);
        set => this.SetInt(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public IEnumerable<ValidationMessage> Validate()
    {
        // SE segment 2nd element must match ST segment
        if (Parent is Loops._834 transaction &&
            TransactionSetControlNumber != transaction.ST.TransactionSetControlNumber)
        {
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Critical,
                "SE segment control number must match ST segment control number");
        }
    }
}