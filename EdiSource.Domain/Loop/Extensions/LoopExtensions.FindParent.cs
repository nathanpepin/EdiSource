using EdiSource.Domain.Exceptions;
using EdiSource.Domain.Helper;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// Finds and returns the parent loop of the specified type for a given loop.
    /// <param name="loop">The loop for which the parent is to be found.</param>
    /// <param name="avoidCircularReferences">
    ///     Specifies whether to avoid circular references to prevent infinite loops.
    ///     Defaults to true.
    /// </param>
    /// <param name="maxIterations">The maximum number of iterations allowed when avoiding circular references.</param>
    /// <typeparam name="T">The type of the parent loop to be found.</typeparam>
    /// <return>The parent loop of the specified type if found; otherwise, the default value for the type.</return>
    public static T? FindParent<T>(this ILoop loop, bool avoidCircularReferences = true, int maxIterations = 1_000)
        where T : class, ILoop
    {
        if (loop is T e) return e;

        var l = loop.GetParentGeneric();

        var iterations = 0;
        while (true)
        {
            switch (l)
            {
                case null:
                    return null;
                case T value:
                    return value;
            }

            l = loop.GetParentGeneric();

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new ProbableCircularReferenceException(iterations, maxIterations);
        }
    }
}