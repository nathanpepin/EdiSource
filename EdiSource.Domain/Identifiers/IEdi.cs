using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Denotes an edi element used for patten matching.
///     It should only have three primary inheritors:<br />
///     - Segment<br />
///     - SegmentList<br />
///     - Loop<br />
///     - LoopList<br />
/// </summary>
public interface IEdi;

public interface IEdi<T> : IEdi where T : IEdi
{
    /// <summary>
    /// Validations that a user can supply that will be picked up by the validator.
    ///
    /// If add validations in a library, it is recommended to use a static
    /// constructor to ensure they are added.
    ///
    /// <code>
    /// static GS()
    /// {
    ///     ValidationHelper.Add&lt;GS>(x => x.GetCompositeElementOrNull(0) is null
    ///         ? [ValidationFactory.CreateCritical(x, "This makes no sense")]
    ///         : null);
    /// }
    /// </code>
    /// </summary>
    static List<IIndirectValidatable<T>> Validations { get; set; } = [];
   
    /// <summary>
    /// Reserved for validations that are generated from a source generator.
    /// </summary>
    List<IIndirectValidatable> SourceGenValidations { get; set; }
}