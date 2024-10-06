namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    /// Yields the child loops of the specified loop, optionally in a recursive manner.
    /// </summary>
    /// <typeparam name="T">The type of the loop, which must implement <see cref="ILoop"/>.</typeparam>
    /// <param name="it">The instance of the loop from which to yield child loops.</param>
    /// <param name="recursive">A flag indicating whether to yield child loops recursively.</param>
    /// <returns>An enumerable collection of child loops.</returns>
    public static List<ILoop> YieldChildLoops<T>(this T it, bool recursive = true)
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