using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Segments.DTPData;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Domain.Validation.Validator;
using EdiSource.Loops;
using Spectre.Console;

namespace EdiSource.Segments;

[ElementLength(ValidationSeverity.Critical, 0, 20)]
[SegmentGenerator<_834, DTP>("DTP", null)]
public partial class TS_DTP : IValidatable
{
    static TS_DTP()
    {
        ValidationHelper.Add(IUserValidation<DTP>.UserValidations.ConvertMultiple<DTP, TS_DTP>());
        ValidationHelper.Add((TS_ST x) =>
        {
            if (x.GetCompositeElement(0, 0) is "ST")
            {
                return [ValidationFactory.CreateInfo(x, "This is an ST")];
            }

            return [];
        });

    }

    public new IEnumerable<ValidationMessage> Validate()
    {
        foreach (var v in base.Validate())
            yield return v;
        
        yield return ValidationFactory.CreateCritical(this, "What");
    }
}
