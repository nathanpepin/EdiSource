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