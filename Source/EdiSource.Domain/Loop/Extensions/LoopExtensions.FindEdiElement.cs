namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    // /// <summary>
    // ///     Finds all EDI elements of a specified type within the given loop.
    // /// </summary>
    // /// <typeparam name="T">The type of EDI elements to find.</typeparam>
    // /// <param name="it">The loop to search within.</param>
    // /// <returns>A collection of EDI elements of the specified type.</returns>
    // public static List<T> FindEdiElement<T>(this ILoop it) where T : IEdi
    // {
    //     List<T> output = [];
    //     var type = typeof(T);
    //     var found = false;
    //
    //     EdiAction(it,
    //         x =>
    //          {
    //             if (found || x.GetType() != type) return;
    //
    //             output.Add((T)x);
    //             found = true;
    //         },
    //         segmentList =>
    //         {
    //             if (found) return;
    //
    //             foreach (var segment in segmentList)
    //             {
    //                 if (segment.GetType() != type) continue;
    //
    //                 output.Add((T)segment);
    //                 found = true;
    //             }
    //         },
    //         loop =>
    //         {
    //             if (found) return;
    //             
    //             if (loop.GetType() == type)
    //             {
    //                 output.Add((T)loop);
    //                 found = true;
    //                 return;
    //             }
    //
    //             loop.FindEdiElement<T>();
    //         },
    //         loopList =>
    //         {
    //             if (found) return;
    //
    //             foreach (var loop in loopList)
    //             {
    //                 if (loop.GetType() == type)
    //                 {
    //                     output.Add((T)loop);
    //                     found = true;
    //                 }
    //
    //                 if (found) return;
    //                 loop.FindEdiElement<T>();
    //             }
    //         });
    //
    //     return output;
    // }

    /// <summary>
    ///     Finds all EDI elements of a specified type within the given loop.
    /// </summary>
    /// <typeparam name="T">The type of EDI elements to find.</typeparam>
    /// <param name="it">The loop to search within.</param>
    /// <param name="output"></param>
    /// <returns>A collection of EDI elements of the specified type.</returns>
    public static List<T> FindEdiElement<T>(this ILoop it, List<T>? output = null) where T : IEdi
    {
        output ??= [];

        foreach (var item in it.EdiItems)
            switch (item)
            {
                case T t:
                    output.Add(t);
                    return output;
                case IEnumerable<T> ts:
                    output.AddRange(ts);
                    return output;
                case IEnumerable<Segment> segments:
                    foreach (var segment in segments)
                        if (segment is T t)
                            output.Add(t);

                    if (output.Count > 0) return output;

                    break;
                case ILoop loop:
                    loop.FindEdiElement(output);
                    if (output.Count > 0) return output;
                    break;
                case IEnumerable<ILoop> loops:
                    foreach (var loop in loops)
                        if (loop is T t)
                            output.Add(t);
                        else
                            loop.FindEdiElement(output);

                    if (output.Count > 0) return output;

                    break;
            }

        return [];
    }
}