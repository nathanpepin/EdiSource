namespace EdiSource.Domain.Standard.Date;

public static class DateFormats
{
    public static readonly HashSet<string> StandardFormats =
    [
        "yy", "MMMYYYY", "yyyyMM", "yyyy'Q'q", "yyyy", "yyMMdd", "yyyyMMdd",
        "dd", "MMddyyyy", "yyyyMMddHHmm", "yyyyMMddHHmmss", "yDDD",
        "yyMMMdd", "MMyyyy", "MMdd", "MM", "HHmm", "DDD", "HHmmss",
        "ddMMyyHHmm", "MMddyy", "yyDDD", "", "yyyyMMM", "MMM"
    ];
}