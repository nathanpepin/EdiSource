namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
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
}