using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
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
}