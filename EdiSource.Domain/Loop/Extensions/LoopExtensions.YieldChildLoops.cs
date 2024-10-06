namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
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
}