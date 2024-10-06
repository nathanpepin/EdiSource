using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop;

/// <summary>
///     Notes a list of loops.
///     In practice, it is a List of ILoop with an IEdi interface for pattern matching purposes.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class LoopList<T> : List<T>, IEdi
    where T : ILoop;