using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    ///     Finds all EDI elements of a specified type within the given loop.
    /// </summary>
    /// <typeparam name="T">The type of EDI elements to find.</typeparam>
    /// <param name="it">The loop to search within.</param>
    /// <returns>A collection of EDI elements of the specified type.</returns>
    public static List<T> FindEdiElement<T>(this ILoop it) where T : IEdi
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