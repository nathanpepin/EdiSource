using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, Loop2100, Loop2100_NM1>]
public partial class Loop2100
{
    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; }

}

/*
public static async Task<Loop2100> InitializeAsync(ChannelReader<ISegment> segmentReader, TransactionSet? parent)
   {
               var loop = new Loop2100();
               loop.Parent = parent;
   
               loop.NM1 = await SegmentLoopFactory<EdiSource.Segments.Loop2100_NM1, Loop2100>.CreateAsync(segmentReader, loop);
   
               return loop;
           }
           */