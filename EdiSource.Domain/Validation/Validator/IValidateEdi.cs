using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Validator;

public interface IValidateEdi
{
    ValidationResult Validate<T>(T ediItem)
        where T : IEdi;
}