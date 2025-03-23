namespace EdiSource.Domain.Helper;

public static class ParentHelpers
{
    public static ILoop? GetParentGeneric<T>(this T edi) where T : IEdi
    {
        return edi is IEdi<T> e ? (ILoop?)e.Parent : null;
    }
}