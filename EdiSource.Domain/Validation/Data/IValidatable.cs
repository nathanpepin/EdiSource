namespace EdiSource.Domain.Validation.Data;

/// <summary>
/// Denotes an IEdi item that can be validated
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    IEnumerable<ValidationMessage> Validate();
}