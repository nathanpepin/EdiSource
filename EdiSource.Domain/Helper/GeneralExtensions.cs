namespace EdiSource.Domain.Helper;

internal static class GeneralExtensions
{
    public static TR Map<T, TR>(this T it, Func<T, TR> fun) => fun(it);
}