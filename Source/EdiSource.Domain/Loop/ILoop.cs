namespace EdiSource.Domain.Loop;

/// <summary>
///     Denotes a loop
/// </summary>
public interface ILoop : IEdi
{
    /// <summary>
    ///     The items of the loop including:<br />
    ///     - Segment<br />
    ///     - Segment lists<br />
    ///     - Loops<br />
    ///     - Loop lists<br />
    ///     <br />
    ///     Used for dynamic recursive calls.
    /// </summary>
    List<IEdi?> EdiItems { get; }
}