using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public interface ILoop : IEdi
{
    ILoop? Parent { get; }
    List<IEdi?> EdiItems { get; }
}

public interface ILoop<out TParent> : ILoop where TParent : ILoop
{
    new TParent? Parent { get; }
}

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