namespace EdiSource.Domain.Validation.Data;

/// <summary>
/// Denotes an IEdi item that's validation functions
/// will be source generated
/// </summary>
public interface ISourceGeneratorValidatable
{
    /// <summary>
    /// Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    IEnumerable<ValidationMessage> Validate();
}