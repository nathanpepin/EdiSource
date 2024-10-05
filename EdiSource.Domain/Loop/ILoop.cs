using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop;

public interface ILoop : IEdi
{
    ILoop? Parent { get; }
    List<IEdi?> EdiItems { get; }
}

public interface ILoop<out TParent> :
    ILoop 
    where TParent : ILoop
{
    new TParent? Parent { get; }
}