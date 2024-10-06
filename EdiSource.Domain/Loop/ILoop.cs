using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop;

/// <summary>
/// Denotes a loop
/// </summary>
public interface ILoop : IEdi
{
    /// <summary>
    /// The loop's parent.
    /// Should be null is the loop is the root.
    /// </summary>
    ILoop? Parent { get; }

    /// <summary>
    /// The items of the loop including:<br/>
    /// - Segment<br/>
    /// - Segment lists<br/>
    /// - Loops<br/>
    /// - Loop lists<br/>
    ///<br/>
    /// Used for dynamic recursive calls.
    /// </summary>
    List<IEdi?> EdiItems { get; }
}

/// <summary>
/// Types a loop with a parent of a specific type
/// </summary>
/// <typeparam name="TParent"></typeparam>
public interface ILoop<out TParent> :
    ILoop
    where TParent : ILoop
{
    /// <summary>
    /// The loop's parent.
    /// Should be null is the loop is the root.
    /// </summary>
    new TParent? Parent { get; }
}