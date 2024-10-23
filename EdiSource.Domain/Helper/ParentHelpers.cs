using EdiSource.Domain.Loop;

namespace EdiSource.Domain.Identifiers;

public static class ParentHelpers
{
    public static ILoop? GetParentGeneric<T>(this T edi) where T : IEdi
    {
        return edi is IEdi<T> e ? (ILoop?)e.Parent : null;
    }
}