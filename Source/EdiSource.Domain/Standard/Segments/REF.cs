namespace EdiSource.Domain.Standard.Segments;

public class REF : Segment, IValidatable, ISourceGeneratorValidatable
{
    public static EdiId EdiId { get; } = new("REF");

    public string? E01Identifier
    {
        get => GetCompositeElementOrNull(1);
        set => SetCompositeElement(value ?? string.Empty, 1);
    }

    public string? E02Identification
    {
        get => GetCompositeElementOrNull(2, 2);
        set => value?.Do(x => SetCompositeElement(x, 1));
    }

    public Element? E03Description
    {
        get => GetElementOrNull(3);
        set => SetDataElement(3, values: [..value?.Select(x => x) ?? []]);
    }

    public List<IIndirectValidatable> SourceGenValidations { get; } =
    [
        new RequiredDataElementsAttribute(ValidationSeverity.Critical, [0, 1]),
        new IsOneOfValuesAttribute(ValidationSeverity.Critical, 0, 0, "REF"),
        new ElementLengthAttribute(ValidationSeverity.Critical, 1, 80),
        new ElementLengthAttribute(ValidationSeverity.Critical, 2, 80),
        new ElementLengthAttribute(ValidationSeverity.Critical, 2, 80),
        new ElementLengthAttribute(ValidationSeverity.Critical, 3, 80)
    ];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (E02Identification is null && E03Description is null)
            yield return ValidationFactory.CreateError(this, "REF required either 2 or 3 elements");
    }
}