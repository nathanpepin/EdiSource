using EdiSource.Domain.Exceptions;
using EdiSource.Domain.Helper;
using EdiSource.Domain.Standard.Loops.ISA;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    ///     Retrieves the root <see cref="ILoop" /> instance from the current loop.
    /// </summary>
    /// <param name="loop">The initial loop to start from.</param>
    /// <param name="avoidCircularReferences">
    ///     Specifies whether to avoid circular references by limiting the number of
    ///     iterations.
    /// </param>
    /// <param name="maxIterations">The maximum number of iterations allowed when avoiding circular references.</param>
    /// <returns>The root <see cref="ILoop" /> instance.</returns>
    /// <exception cref="Exception">
    ///     Thrown if the loop iteration exceeds <paramref name="maxIterations" />, indicating a
    ///     potential circular reference.
    /// </exception>
    public static InterchangeEnvelope? GetRoot(this ILoop loop, bool avoidCircularReferences = true,
        int maxIterations = 1_000)
    {
        if (loop is InterchangeEnvelope e) return e;

        var l = loop.GetParentGeneric();

        var iterations = 0;
        while (true)
        {
            switch (l)
            {
                case null:
                    return null;
                case InterchangeEnvelope envelope:
                    return envelope;
            }

            l = loop.GetParentGeneric();

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new ProbableCircularReferenceException(iterations, maxIterations);
        }
    }
}