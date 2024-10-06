namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Denotes an IEdi element that has an identifier.
///     Used for static abstract interfaces.
/// </summary>
public interface IEdiId : IEdi, ISegmentIdentifier;