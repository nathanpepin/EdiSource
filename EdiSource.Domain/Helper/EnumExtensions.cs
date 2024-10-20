namespace EdiSource.Domain.Helper;

public static class EnumExtensions
{
    private static readonly Dictionary<Type, string[]> EnumLookup = new();

    public static string[] EnumToStringArray<T>(bool removeUnderscore = true) where T : Enum
    {
        var type = typeof(T);

        return EnumLookup.TryGetValue(type, out var result)
            ? result
            : EnumLookup[type] = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(code => code
                    .ToString()
                    .ApplyIf(x => x.Replace('_', ' '), removeUnderscore))
                .ToArray();
    }
}