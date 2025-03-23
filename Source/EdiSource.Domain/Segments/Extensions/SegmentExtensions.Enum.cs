namespace EdiSource.Domain.Segments.Extensions;

public static partial class SegmentExtensions
{
    /// <summary>
    ///     Attempts to parse an enum from an element via enum name.
    ///     If tryAddUnderscore is true, it will also try an enum name with
    ///     an '_' prefix to enable enums that start with not allowed characters.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="tryAddUnderscore"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum? GetEnum<TEnum>(this Segment it, int dataElement, int compositeElement = 0,
        bool tryAddUnderscore = true)
        where TEnum : struct, Enum
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return Enum.TryParse<TEnum>(element, out var value)
            ? value
            : tryAddUnderscore
                ? Enum.TryParse($"_{element}", out value)
                    ? value
                    : null
                : null;
    }

    /// <summary>
    ///     Attempts to parse an enum from an element via a mapping function.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="fun"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum? GetEnum<TEnum>(this Segment it, Func<string, TEnum> fun, int dataElement,
        int compositeElement = 0)
        where TEnum : struct, Enum
    {
        if (it.GetCompositeElementOrNull(dataElement, compositeElement) is not { } element) return null;

        return fun(element);
    }

    /// <summary>
    ///     Attempts to parse an enum from an element via a mapping function.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="fun"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum GetEnumRequired<TEnum>(this Segment it, Func<string, TEnum> fun, int dataElement,
        int compositeElement = 0)
        where TEnum : struct, Enum
    {
        return GetEnum(it, fun, dataElement, compositeElement) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(TEnum));
    }


    /// <summary>
    ///     Attempts to parse an enum from an element via enum name.
    ///     If tryAddUnderscore is true, it will also try an enum name with
    ///     an '_' prefix to enable enums that start with not allowed characters.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="tryAddUnderscore"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum GetEnumRequired<TEnum>(this Segment it, int dataElement, int compositeElement = 0,
        bool tryAddUnderscore = true)
        where TEnum : struct, Enum
    {
        return GetEnum<TEnum>(it, dataElement, compositeElement, tryAddUnderscore) ??
               throw new DataElementParsingError(dataElement, compositeElement, typeof(TEnum));
    }

    /// <summary>
    ///     Attempts to set an enum using a basic to string conversion.
    ///     If tryRemoveUnderscore is true, it will remove the '_' from the enum name
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create"></param>
    /// <param name="tryRemoveUnderscore"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static bool SetEnum<TEnum>(this Segment it, TEnum value, int dataElement, int compositeElement = 0,
        bool create = true, bool tryRemoveUnderscore = true)
        where TEnum : struct, Enum
    {
        var text = value.ToString().AsSpan();
        if (tryRemoveUnderscore && text[0] == '_') text = text[1..];

        return it.SetCompositeElement(text.ToString(), dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set an enum using a basic to string conversion.
    ///     If tryRemoveUnderscore is true, it will remove the '_' from the enum name
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create"></param>
    /// <param name="tryRemoveUnderscore"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static bool SetEnum<TEnum>(this Segment it, TEnum? value, int dataElement, int compositeElement = 0,
        bool create = true, bool tryRemoveUnderscore = true)
        where TEnum : struct, Enum
    {
        if (value is null) return false;

        var text = value.ToString().AsSpan();
        if (tryRemoveUnderscore && text[0] != '_') text = text[1..];

        return it.SetCompositeElement(text.ToString(), dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set an enum using a mapping function.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="fun">The mapping function</param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static bool SetEnum<TEnum>(this Segment it, TEnum value, Func<TEnum, string> fun, int dataElement,
        int compositeElement = 0,
        bool create = true)
        where TEnum : struct, Enum
    {
        return it.SetCompositeElement(fun(value), dataElement, compositeElement, create);
    }

    /// <summary>
    ///     Attempts to set an enum using a mapping function.
    /// </summary>
    /// <param name="it"></param>
    /// <param name="value"></param>
    /// <param name="fun">The mapping function</param>
    /// <param name="dataElement"></param>
    /// <param name="compositeElement"></param>
    /// <param name="create"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static bool SetEnum<TEnum>(this Segment it, TEnum? value, Func<TEnum, string> fun, int dataElement,
        int compositeElement = 0,
        bool create = true)
        where TEnum : struct, Enum
    {
        return value is not null
               && it.SetCompositeElement(fun(value.Value), dataElement, compositeElement, create);
    }
}