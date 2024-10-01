using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator]
public class TransactionSet : ILoop<TransactionSet>, ISegmentIdentifier<TS_ST>, ISegmentIdentifier<TransactionSet>
{
    public TransactionSet(Queue<ISegment> segments)
    {
        //Header
        ST = SegmentFactory<TS_ST>.Create(segments);

        //Body
        while (segments.Count > 0)
        {
            if (ISegmentIdentifier<TS_REF>.Matches(segments))
            {
                REFs.Add(SegmentFactory<TS_REF>.Create(segments));
                continue;
            }

            if (ISegmentIdentifier<TS_DTP>.Matches(segments))
            {
                if (DTP is not null) throw new Exception("Already assigned");
                DTP = SegmentFactory<TS_DTP>.Create(segments);
                continue;
            }

            if (ISegmentIdentifier<Loop2000>.Matches(segments))
            {
                if (Loop2000 is not null) throw new Exception("Already assigned");
                Loop2000 = new Loop2000(segments);
                continue;
            }

            if (ISegmentIdentifier<Loop2100>.Matches(segments))
            {
                Loop2100s.Add(new Loop2100(segments));
                continue;
            }

            break;
        }

        //Footer
        SE = SegmentFactory<TS_SE>.Create(segments);
    }

    public TransactionSet()
    {
    }

    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public List<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; }

    [LoopList] public List<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; }

    ILoop? ILoop.Parent => Parent;
    public TransactionSet? Parent => null;

    public IEnumerable<ISegment> YieldChildSegments()
    {
        yield return ST;

        foreach (var segment in REFs)
        {
            yield return segment;
        }
        
        yield return DTP;

        foreach (var segment in Loop2000.RecursiveYieldSegments())
        {
            yield return segment;
        }

        foreach (var segment in Loop2100s.SelectMany(loop2100 => loop2100.RecursiveYieldSegments()))
        {
            yield return segment;
        }

        yield return SE;
    }

    public IEnumerable<ILoop> YieldChildLoops()
    {
        yield return Loop2000;

        foreach (var loop in Loop2100s)
        {
            yield return loop;
        }
    }

    public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
}