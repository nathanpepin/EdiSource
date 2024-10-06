namespace EdiSource.Domain.Identifiers;

/// <summary>
/// Denotes an edi element used for patten matching.
/// 
/// It should only have three primary inheritors:
///     - Segment
///     - SegmentList
///     - Loop
///     - LoopList
/// </summary>
public interface IEdi;