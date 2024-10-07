//HintName: TransactionSet.QueueConstructor.g.cs
#nullable enable
using EdiSource.Domain.Separator;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Loop;
using EdiSource.Loops;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Test
{
    public partial class TransactionSet
    {
        public TransactionSet(IEnumerable<ISegment> segments, TransactionSet? parent = null)
        : this(new Queue<ISegment>(segments), parent)
        {}

        public TransactionSet(Queue<ISegment> segments, TransactionSet? parent = null)
        {
            ST = SegmentLoopFactory<TS_ST, TransactionSet>.Create(segments, this);

            while (segments.Count > 0)
            {
                if (ISegmentIdentifier<TS_DTP>.Matches(segments))
                {
                    DTP = SegmentLoopFactory<TS_DTP, TransactionSet>.Create(segments, this);
                    continue;
                }
                if (ISegmentIdentifier<TS_REF>.Matches(segments))
                {
                    REFs.Add(SegmentLoopFactory<TS_REF, TransactionSet>.Create(segments, this));
                    continue;
                }
                if (ISegmentIdentifier<Loop2000>.Matches(segments))
                {
                    Loop2000 = new Loop2000(segments, this);
                    continue;
                }
                if (ISegmentIdentifier<Loop2100>.Matches(segments))
                {
                    Loop2100s.Add(new Loop2100(segments, this));
                    continue;
                }
                if (ISegmentIdentifier<TS_FSE>.Matches(segments))
                {
                    SE = SegmentLoopFactory<TS_FSE, TransactionSet>.Create(segments, this);
                    break;
                }

                break;
            }

            SE = SegmentLoopFactory<TS_SE, TransactionSet>.Create(segments, this);
        }
    }
}
