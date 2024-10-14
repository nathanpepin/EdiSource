namespace EdiSource.Domain.Helper;

internal static class GeneralExtensions
{
    public static TR Map<T, TR>(this T it, Func<T, TR> fun)
    {
        return fun(it);
    }

    public static T Do<T>(this T it, Action<T> action)
    {
        action(it);
        return it;
    }

    public static Task<TR> MapAsync<T, TR>(this T it, Func<T, Task<TR>> fun)
    {
        return fun(it);
    }

    public static async Task<T> DoAsync<T>(this T it, Func<T, Task> action)
    {
        await action(it);
        return it;
    }


    public static bool InsideBounds<T>(this IList<T> it, int elementIndex)
    {
        return elementIndex < it.Count && elementIndex >= 0;
    }

    public static bool InsideBoundsTiered<T>(this IList<IList<T>> it, int first, int second)
    {
        return it.InsideBounds(first) && it[first].InsideBounds(second);
    }

    public static bool InsideBounds<T>(this T[] it, int elementIndex)
    {
        return elementIndex < it.Length && elementIndex >= 0;
    }

    public static bool InsideBoundsTiered<T>(this T[][] it, int first, int second)
    {
        return it.InsideBounds(first) && it[first].InsideBounds(second);
    }
}