using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator]
[LoopGenerator<TransactionSet, TS_ST>]
public partial class TransactionSet : ILoop<TransactionSet>, ISegmentIdentifier<TS_ST>,
    ISegmentIdentifier<TransactionSet>
{
    // public TransactionSet(Queue<ISegment> segments)
    // {
    //     //Header
    //     ST = SegmentLoopFactory<TS_ST, TransactionSet>.Create(segments, this);
    //
    //     //Body
    //     while (segments.Count > 0)
    //     {
    //         if (ISegmentIdentifier<TS_REF>.Matches(segments))
    //         {
    //             REFs.Add(SegmentLoopFactory<TS_REF, TransactionSet>.Create(segments, this));
    //             continue;
    //         }
    //
    //         if (ISegmentIdentifier<TS_DTP>.Matches(segments))
    //         {
    //             if (DTP is not null) throw new Exception("Already assigned");
    //             DTP = SegmentLoopFactory<TS_DTP, TransactionSet>.Create(segments, this);
    //             continue;
    //         }
    //
    //         if (ISegmentIdentifier<Loop2000>.Matches(segments))
    //         {
    //             if (Loop2000 is not null) throw new Exception("Already assigned");
    //             Loop2000 = new Loop2000(segments, this);
    //             continue;
    //         }
    //
    //         if (ISegmentIdentifier<Loop2100>.Matches(segments))
    //         {
    //             Loop2100s.Add(new Loop2100(segments, this));
    //             continue;
    //         }
    //
    //         break;
    //     }
    //
    //     //Footer
    //     SE = SegmentLoopFactory<TS_SE, TransactionSet>.Create(segments, this);
    // }
    
    public TransactionSet()
    {
        for (var index = 0; index < HeaderSegments.Count; index++)
        {
            var headerSegment = HeaderSegments[index];
            ref ISegment? s = ref headerSegment;

            if (s is ISegmentIdentifier i)
            {

            }
        }
    }
    

    private List<ISegment> HeaderSegments => [ST];

    private List<IEdi> BodyEdi => [REFs, DTP, Loop2000, Loop2100s];

    private List<ISegment> FooterSegments => [SE];

    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; }

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; }

    ILoop? ILoop.Parent => Parent;
    public TransactionSet? Parent => null;

    public IEnumerable<ISegment> YieldChildSegments()
    {
        List<ISegment> items = [];
        RecursiveSegmentAction(x => items.Add(x));
        return items;
    }

    public IEnumerable<ISegment> YieldDirectChildSegments()
    {
        List<ISegment> items = [];
        EdiAction(
            segmentAction: segment => items.Add(segment),
            segmentListAction: segmentList => items.AddRange(segmentList));
        return items;
    }

    public void EdiAction(Action<ISegment>? segmentAction = null,
        Action<SegmentList<ISegment>>? segmentListAction = null,
        Action<ILoop>? loopAction = null, Action<LoopList<ILoop>>? loopListAction = null)
    {
        foreach (var ediItem in EdiItems)
        {
            switch (ediItem)
            {
                case null: continue;
                case ISegment segment:
                    segmentAction?.Invoke(segment);
                    continue;
                case SegmentList<ISegment> segmentList:
                    segmentListAction?.Invoke(segmentList);
                    break;
                case ILoop loop:
                    loopAction?.Invoke(loop);
                    break;
                case LoopList<ILoop> loopList:
                    loopListAction?.Invoke(loopList);
                    break;
            }
        }
    }

    public void RecursiveSegmentAction(Action<ISegment> action)
    {
        EdiAction(
            segmentAction: action,
            segmentListAction: segmentList =>
            {
                foreach (var segment in segmentList)
                {
                    action(segment);
                }
            },
            loopAction: loop =>
            {
                foreach (var childSegment in loop.YieldChildSegments())
                {
                    action(childSegment);
                }
            },
            loopListAction: loopList =>
            {
                foreach (var childSegment in loopList.SelectMany(x => x.YieldChildSegments()))
                {
                    action(childSegment);
                }
            });
    }

    public IEnumerable<ILoop> YieldDirectChildLoops()
    {
        List<ILoop> items = [];
        EdiAction(
            loopAction: loop => items.Add(loop),
            loopListAction: loopList => items.AddRange(loopList));
        return items;
    }

    public IEnumerable<ILoop> YieldChildLoops()
    {
        List<ILoop> items = [];
        EdiAction(
            loopAction: loop =>
            {
                items.Add(loop);
                items.AddRange(loop.YieldChildLoops());
            },
            loopListAction: loopList =>
            {
                items.AddRange(loopList);
                items.AddRange(loopList.SelectMany(x => x.YieldChildLoops()));
            });
        return items;
    }

    public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
}