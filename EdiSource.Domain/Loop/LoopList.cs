using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop;

public sealed class LoopList<T> : List<T>, IEdi
    where T : ILoop;