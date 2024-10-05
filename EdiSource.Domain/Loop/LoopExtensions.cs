using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Loop;

public static class LoopExtensions
{
    public static void EdiAction<T>(this T it, Action<ISegment>? segmentAction = null,
        Action<SegmentList<ISegment>>? segmentListAction = null,
        Action<ILoop>? loopAction = null, Action<LoopList<ILoop>>? loopListAction = null)
        where T : ILoop
    {
        foreach (var ediItem in it.EdiItems)
            switch (ediItem)
            {
                case null: continue;
                case ISegment segment:
                    segmentAction?.Invoke(segment);
                    continue;
                case IEnumerable<ISegment> segmentList:
                    segmentListAction?.Invoke([..segmentList]);
                    continue;
                case ILoop loop:
                    loopAction?.Invoke(loop);
                    continue;
                case IEnumerable<ILoop> loopList:
                    loopListAction?.Invoke([..loopList]);
                    continue;
            }
    }

    public static IEnumerable<ISegment> YieldChildSegments<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<ISegment> items = [];
        SegmentAction(it, x => items.Add(x), recursive);
        return items;
    }

    public static void SegmentAction<T>(this T it, Action<ISegment> action, bool recursive = true)
        where T : ILoop
    {
        EdiAction(it,
            action,
            segmentList =>
            {
                foreach (var segment in segmentList) action(segment);
            },
            loop =>
            {
                if (!recursive) return;
                foreach (var childSegment in loop.YieldChildSegments()) action(childSegment);
            },
            loopList =>
            {
                if (!recursive) return;
                foreach (var childSegment in loopList.SelectMany(x => x.YieldChildSegments())) action(childSegment);
            });
    }

    public static IEnumerable<ILoop> YieldChildLoops<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<ILoop> items = [];
        EdiAction(it,
            loopAction: loop =>
            {
                items.Add(loop);

                if (recursive)
                    items.AddRange(loop.YieldChildLoops());
            },
            loopListAction: loopList =>
            {
                items.AddRange(loopList);

                if (recursive)
                    items.AddRange(loopList.SelectMany(x => x.YieldChildLoops()));
            });
        return items;
    }

    public static IEnumerable<T> FindEdiElement<T>(this ILoop it) where T : IEdi
    {
        List<T> output = [];

        EdiAction(it,
            x =>
            {
                if (x is T t)
                    output.Add(t);
            },
            segmentList =>
            {
                foreach (var segment in segmentList)
                    if (segment is T t)
                        output.Add(t);
            },
            loop =>
            {
                if (loop is T t)
                {
                    output.Add(t);
                    return;
                }

                loop.FindEdiElement<T>();
            },
            loopList =>
            {
                foreach (var loop in loopList)
                {
                    var found = false;

                    if (loop is T t)
                    {
                        output.Add(t);
                        found = true;
                    }

                    if (found) return;

                    loop.FindEdiElement<T>();
                }
            });

        return output;
    }

    public static ILoop GetRoot(this ILoop loop, bool avoidCircularReferences = true)
    {
        var iterations = 0;
        const int maxIterations = 1_000;

        while (true)
        {
            if (loop.Parent is null) return loop;
            loop = loop.Parent;

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new Exception($"Loop has more than {maxIterations} iterations, likely a circular reference.");
        }
    }

    public static T? FindParent<T>(this ILoop loop, bool avoidCircularReferences = true) where T : ILoop
    {
        var iterations = 0;
        const int maxIterations = 1_000;

        while (true)
        {
            if (loop is T foundLoop) return foundLoop;

            if (loop.Parent is null) return default;

            loop = loop.Parent;

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new Exception($"Loop has more than {maxIterations} iterations, likely a circular reference.");
        }
    }
}