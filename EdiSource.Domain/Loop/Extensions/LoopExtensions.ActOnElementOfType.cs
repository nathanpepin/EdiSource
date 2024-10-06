using EdiSource.Domain.Identifiers;

namespace EdiSource.Domain.Loop.Extensions;

public static partial class LoopExtensions
{
    /// <summary>
    /// Applies an action to matching elements of type T
    /// </summary>
    /// <param name="it"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ActOnElementOfType<T>(this ILoop it, Action<T> action) where T : IEdi
    {
        var elements = it.FindEdiElement<T>();

        foreach (var element in elements)
        {
            action(element);
        }
    }
}