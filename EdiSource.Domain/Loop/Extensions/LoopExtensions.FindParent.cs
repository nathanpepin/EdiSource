namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
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