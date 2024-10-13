using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Standard.Loops;

public interface ITransactionSet : ILoop
{
    static abstract Func<(string, string?),
        Func<ChannelReader<ISegment>, FunctionalGroup, Task<ILoop>>?> Definition { get; }
}

public interface ITransactionSet<TSelf, TId> :
    ITransactionSet,
    ILoop<FunctionalGroup>, ISegmentIdentifier<TId>,
    ILoopInitialize<FunctionalGroup, TSelf>
    where TSelf : ITransactionSet<TSelf, TId>
    where TId : ISegmentIdentifier<TId>
{
    static abstract Func<(string, string?),
        Func<ChannelReader<ISegment>, FunctionalGroup, Task<ILoop>>?> Definition { get; }
}