namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2100>("NM1")]
public partial class Loop2100_NM1 : IValidatable
{
    public string EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string EntityTypeQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string LastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string FirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? MiddleName
    {
        get => GetCompositeElementOrNull(5);
        set => value?.Do(x => SetCompositeElement(x, 5));
    }

    public IEnumerable<ValidationMessage> Validate()
    {
        // Entity Identifier Code IL = Insured
        if (EntityIdentifierCode != "IL" && EntityTypeQualifier == "1")
            yield return ValidationFactory.Create(
                this,
                ValidationSeverity.Warning,
                "For individual members, entity identifier code should be 'IL'");
    }
}