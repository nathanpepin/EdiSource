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
}