using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Standard.Loops;

public delegate Func<ChannelReader<Segment>, FunctionalGroup, Task<ILoop>>?
    TransactionSetDefinition(Segment id);

public static class TransactionSetDefinitionsFactory<T>
    where T : ILoop, ILoopInitialize<FunctionalGroup, T>, ISegmentIdentifier<T>
{
    public static TransactionSetDefinition CreateDefinition()
    {
        return id =>
        {
            if (!T.EdiId.MatchesSegment(id)) return null;

            return (segmentReader, parent) =>
                T.InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
        };
    }
}