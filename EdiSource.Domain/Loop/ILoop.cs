using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public interface ILoop : IEdi
{
    ILoop? Parent { get; }
    // IEnumerable<ISegment> YieldChildSegments();
    // IEnumerable<ILoop> YieldChildLoops();
    // public IEnumerable<ILoop> YieldSubLoops()
    // {
    //     yield return this;
    //     
    //     foreach (var subLoop in YieldChildLoops())
    //     {
    //         foreach (var subSubLoop in subLoop.YieldSubLoops())
    //         {
    //             yield return subSubLoop;
    //         }
    //     }
    // }
}

public interface ILoop<out TParent> : ILoop where TParent : ILoop
{
    new TParent? Parent { get; }
}


public interface ILoopInitialize<TSelf> : ILoop where TSelf : ILoop
{
    static abstract Task<TSelf> InitializeAsync(ChannelReader<ISegment> segmentReader, TSelf? parent);
}
