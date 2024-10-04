//HintName: TransactionSet.Constructor.g.cs
using System;
using System.Collections.Generic;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Identifiers;

namespace EdiSource.Loops
{
    public partial class TransactionSet
    {
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
                break;
            }

            SE = SegmentLoopFactory<TS_SE, TransactionSet>.Create(segments, this);
        }
    }
}