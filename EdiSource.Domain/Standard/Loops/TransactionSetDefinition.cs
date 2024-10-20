using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Standard.Loops;

public delegate Func<ChannelReader<ISegment>, FunctionalGroup, Task<ILoop>>?
    TransactionSetDefinition(ISegment id);