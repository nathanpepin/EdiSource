using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Standard.Loops;

public delegate Func<ChannelReader<Segment>, FunctionalGroup, Task<ILoop>>?
    TransactionSetDefinition(Segment id);