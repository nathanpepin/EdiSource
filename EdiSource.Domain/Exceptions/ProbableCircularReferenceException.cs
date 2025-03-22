namespace EdiSource.Domain.Exceptions;

public sealed class ProbableCircularReferenceException(int iterations, int maxIterations)
    : Exception($"Function has iterated {iterations} iterations where max iterations are {maxIterations}, likely a circular reference");