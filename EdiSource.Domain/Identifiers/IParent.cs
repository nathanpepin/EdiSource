using EdiSource.Domain.Loop;

namespace EdiSource.Domain.Identifiers;

public interface IParent
{
    /// <summary>
    /// The parent loop of the edi element if any
    /// </summary>
    ILoop? Parent { get; }
}

public interface IParent<out T> : IParent
    where T : ILoop
{
    /// <summary>
    /// The parent loop of the edi element if any
    /// </summary>
    new T? Parent { get; }
}