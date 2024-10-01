namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<_834>("SE")]
public partial class TS_SE : IValidatable
{
    public int SegmentCount
    {
        get => this.GetIntRequired(1);
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
        if (Parent is _834 transaction &&
            TransactionSetControlNumber != transaction.ST.TransactionSetControlNumber)
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Critical,
                "SE segment control number must match ST segment control number");
    }
}