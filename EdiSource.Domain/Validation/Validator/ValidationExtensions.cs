using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Validator;

public static class ValidationExtensions
{
    public static ValidationResult Validate<T>(this T it)
        where T : IEdi
    {
        var validator = new ValidateEdi();
        return validator.Validate(it);
    }
}