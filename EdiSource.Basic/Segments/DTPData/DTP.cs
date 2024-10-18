using System.ComponentModel.DataAnnotations;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Basic.Segments.DTPData;

[IsOneOfValues(ValidationSeverity.Critical, 0, 0, "DTP")]
[RequiredDataElements(ValidationSeverity.Critical, [0, 1, 2])]
[BeDateTime(ValidationSeverity.Critical, 3, 0)]
[BeDateTime(ValidationSeverity.Critical, 3, 1)]
[ElementLength(ValidationSeverity.Critical, 1, 3)]
[ElementLength(ValidationSeverity.Critical, 2, 3)]
[ElementLength(ValidationSeverity.Critical, 3, 35)]
public partial class DTP : Segment, IValidatable
{
    public string Qualifier
    {
        get => GetCompositeElement(1, 0);
        set => SetCompositeElement(1, 0, value);
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

    public IEnumerable<ValidationMessage> Validate()
    {
        
        return [ValidationFactory.CreateCritical(this, "Fuck")];
    }
}