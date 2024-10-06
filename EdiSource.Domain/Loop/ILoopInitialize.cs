using System.Threading.Channels;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop;

/// <summary>
///     The loop initializing for reading from a channel asynchronously.
///     Used for certain methods that deal with a generic ILoop.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface ILoopInitialize<TSelf> : ILoop where TSelf : ILoop
{
    static abstract Task<TSelf> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent);
}

/// <summary>
///     The loop initializing for reading from a channel asynchronously.
/// </summary>
/// <typeparam name="TParent"></typeparam>
/// <typeparam name="TSelf"></typeparam>
public interface ILoopInitialize<in TParent, TSelf>
    : ILoopInitialize<TSelf>
    where TParent : ILoop
    where TSelf : ILoop
{
    static abstract Task<TSelf> InitializeAsync(ChannelReader<ISegment> segmentReader, TParent? parent);
}