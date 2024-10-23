using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Validation.Data;

/// <summary>
///     Denotes an IEdi item that's validation functions
///     will be source generated
/// </summary>
public interface ISourceGeneratorValidatable
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <returns></returns>
    List<IIndirectValidatable> SourceGenValidations { get; }
}