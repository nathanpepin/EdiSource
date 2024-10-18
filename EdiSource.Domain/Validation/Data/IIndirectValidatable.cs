using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Validation.Data;

/// <summary>
///     Denotes an IEdi item that can be validated
/// </summary>
public interface IIndirectValidatable
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    IEnumerable<ValidationMessage> Validate(IEdi element);
}

public interface IIndirectValidatable<in T> : IIndirectValidatable
    where T : IEdi
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    IEnumerable<ValidationMessage> Validate(T element);
}