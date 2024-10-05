using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<TransactionSet, TransactionSet, TS_ST>]
public partial class TransactionSet
{
    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; }

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; }
}

/*

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
*/