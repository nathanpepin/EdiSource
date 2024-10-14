using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Standard.Loops;

public delegate Func<ChannelReader<ISegment>, FunctionalGroup, Task<ILoop>>? 
    TransactionSetDefinition((string, string?) id);
    
public delegate Func<ChannelReader<ISegment>, FunctionalGroup, Task<T>>? 
    TransactionSetDefinition<T>((string, string?) id)
    where T : ILoop;