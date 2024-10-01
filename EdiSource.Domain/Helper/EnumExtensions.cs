namespace EdiSource.Domain.Helper;

internal static class EnumExtensions
{
    public static string EnumToString<T>(T enumValue, bool trimUnderscore = true) where T : Enum
    {
        return !trimUnderscore
            ? enumValue.ToString()
            : enumValue.ToString().Replace("_", " ");
    }

    public static T StringToEnum<T>(string value)
    {
        return ((T[])Enum.GetValues(typeof(T)))
            .Select(x => (value: x, stringValue: x?.ToString()?.Trim('_')))
            .First(x => x.Item2 == value || x.Item2 == $"_{value}")
            .value;
    }
}