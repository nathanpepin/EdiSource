using System.Threading.Channels;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop;

public interface ILoopInitialize<TSelf> : ILoop where TSelf : ILoop
{
    static abstract Task<TSelf> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent);
}

public interface ILoopInitialize<in TParent, TSelf>
    : ILoopInitialize<TSelf>
    where TParent : ILoop
    where TSelf : ILoop
{
    static abstract Task<TSelf> InitializeAsync(ChannelReader<ISegment> segmentReader, TParent? parent);
}