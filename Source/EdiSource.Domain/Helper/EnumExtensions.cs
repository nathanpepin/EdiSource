namespace EdiSource.Domain.Helper;

public static class EnumExtensions
{
    private static readonly Dictionary<(Type, bool), string[]> EnumLookup = new();

    public static string[] EnumToStringArray<T>(bool removeUnderscoreFromStart = true) where T : Enum
    {
        var type = typeof(T);

        return EnumLookup.TryGetValue((type, removeUnderscoreFromStart), out var result)
            ? result
            : EnumLookup[(type, removeUnderscoreFromStart)] = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(code => code
                    .ToString()
                    .ApplyIf(x => x.TrimStart('_', ' '), removeUnderscoreFromStart))
                .ToArray();
    }
}