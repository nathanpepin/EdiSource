using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Validation.Data;

/// <summary>
///     Denotes an IEdi item that can be validated
/// </summary>
public interface IValidatable
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    IEnumerable<ValidationMessage> Validate();
}

/// <summary>
///     Denotes an IEdi item that can be validated
/// </summary>
public interface IUserValidation<T> : IEdi
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    public static List<Func<T, IEnumerable<ValidationMessage>>> UserValidations { get; } = [];
}