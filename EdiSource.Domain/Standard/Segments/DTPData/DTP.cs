using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Segments.DTPData;

public class DTP : Segment, IValidatable, ISourceGeneratorValidatable
{
    public string Qualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public DateFormatCode DateFormatCode
    {
        get => this.GetEnumRequired<DateFormatCode>(2);
        set => this.SetEnum(value, 2);
    }

    public (string Date, string? DateRange) DateFormats => DateFormatMapper.GetFormat(DateFormatCode);

    public DateTime Date
    {
        get => this.GetDateRequired(3, format: DateFormats.Date);
        set => this.SetDate(value, 3, format: DateFormats.Date);
    }

    public DateTime? DateEnd
    {
        get => DateFormats.DateRange?
            .Map(format => this.GetDate(3, 1, format));
        set => DateFormats.DateRange?
            .Map(format => this.SetDate(value, 3, 1, format));
    }

    public List<IIndirectValidatable> SourceGenValidations { get; } =
    [
        new IsOneOfValuesAttribute(ValidationSeverity.Critical, 0, 0, "DTP"),
        new RequiredDataElementsAttribute(ValidationSeverity.Critical, [0, 1, 2]),
        new BeDateTimeAttribute(ValidationSeverity.Critical, 3, 0),
        new BeDateTimeAttribute(ValidationSeverity.Critical, 3, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 1, 3),
        new ElementLengthAttribute(ValidationSeverity.Critical, 2, 3),
        new ElementLengthAttribute(ValidationSeverity.Critical, 3, 35)
    ];

    public IEnumerable<ValidationMessage> Validate()
    {
        return [ValidationFactory.CreateCritical(this, "Fuck")];
    }
}