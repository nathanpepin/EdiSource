//HintName: TransactionSet.ChannelConstructor.g.cs
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Test
{
    public partial class TransactionSet
    {
        [SuppressMessage("CodeQuality", "IDE0060:Remove unused parameter", Justification = "Empty constructor is required")]
        public TransactionSet()
        {
        }

        public TransactionSet(ChannelReader<ISegment> segmentReader, TransactionSet? parent = null)
        {
            var loop = InitializeAsync(segmentReader, parent).GetAwaiter().GetResult();
            ST = loop.ST;
            DTP = loop.DTP;
            REFs = loop.REFs;
            Loop2000 = loop.Loop2000;
            Loop2100s = loop.Loop2100s;
            SE = loop.SE;
            SE = loop.SE;
        }

        public static Task<TransactionSet> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
        {
            if (parent is null)
            {
                return InitializeAsync(segmentReader, (TransactionSet? )null);
            }

            if (parent is not TransactionSet typedParent)
            {
                throw new ArgumentException($"Parent must be of type TransactionSet");
            }

            return InitializeAsync(segmentReader, typedParent);
        }

        public static async Task<TransactionSet> InitializeAsync(ChannelReader<ISegment> segmentReader, TransactionSet? parent)
        {
            var loop = new TransactionSet();
            loop.ST = await SegmentLoopFactory<TS_ST, TransactionSet>.CreateAsync(segmentReader, loop);
            while (await segmentReader.WaitToReadAsync())
            {
                if (await ISegmentIdentifier<TS_DTP>.MatchesAsync(segmentReader))
                {
                    loop.DTP = await SegmentLoopFactory<TS_DTP, TransactionSet>.CreateAsync(segmentReader, loop);
                    continue;
                }

                if (await ISegmentIdentifier<TS_REF>.MatchesAsync(segmentReader))
                {
                    loop.REFs.Add(await SegmentLoopFactory<TS_REF, TransactionSet>.CreateAsync(segmentReader, loop));
                    continue;
                }

                if (await ISegmentIdentifier<Loop2000>.MatchesAsync(segmentReader))
                {
                    loop.Loop2000 = await Loop2000.InitializeAsync(segmentReader, loop);
                    continue;
                }

                if (await ISegmentIdentifier<Loop2100>.MatchesAsync(segmentReader))
                {
                    loop.Loop2100s.Add(await Loop2100.InitializeAsync(segmentReader, loop));
                    continue;
                }

                if (await ISegmentIdentifier<TS_FSE>.MatchesAsync(segmentReader))
                {
                    loop.SE = SegmentLoopFactory<TS_FSE, TransactionSet>.Create(segments, this);
                    break;
                }

                break;
            }

            loop.SE = await SegmentLoopFactory<TS_SE, TransactionSet>.CreateAsync(segmentReader, loop);
            return loop;
        }
    }
}