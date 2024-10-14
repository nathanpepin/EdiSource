namespace EdiSource.Domain.Validation.Data;

/// <summary>
///     The subject of validation
/// </summary>
[Flags]
public enum ValidationSubject
{
    Unknown = 0,
    Segment = 1,
    Loop = 2
}

public static class ValidationSubjectExtensions
{
    public static string ToEnumString(this ValidationSubject subject)
    {
        return subject switch
        {
            ValidationSubject.Unknown => "Unknown",
            ValidationSubject.Segment => "Segment",
            ValidationSubject.Loop => "Loop",
            _ => "Unknown"
        };
    }

    public static ValidationSubject FromString(string value)
    {
        return value switch
        {
            "Unknown" => ValidationSubject.Unknown,
            "Segment" => ValidationSubject.Segment,
            "Loop" => ValidationSubject.Loop,
            _ => ValidationSubject.Unknown
        };
    }
}