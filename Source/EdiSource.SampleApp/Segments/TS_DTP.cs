namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<_834>("DTP")]
public partial class TS_DTP : IValidatable
{
    public string DateQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DateFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public DateTime Date
    {
        get => this.GetDateRequired(3);
        set => this.SetDate(value, 3);
    }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validate date format qualifier matches actual format
        if (DateFormatQualifier == "D8" &&
            GetCompositeElement(3).Length != 8)
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Error,
                "Date format qualifier D8 requires an 8-digit date");
    }
}