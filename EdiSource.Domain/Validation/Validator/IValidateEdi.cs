using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Validator;

/// <summary>
/// Validates an IEdi item
/// </summary>
public interface IValidateEdi
{
    /// <summary>
    /// Enacts validation on an IEdi item
    /// </summary>
    /// <param name="ediItem"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    EdiValidationResult Validate<T>(T ediItem)
        where T : IEdi;
}