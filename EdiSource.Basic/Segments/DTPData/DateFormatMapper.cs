using System.Globalization;

namespace EdiSource.Basic.Segments.DTPData;

public static class DateFormatMapper
{
    private static readonly Dictionary<DateFormatCode, (string, string?)> _formatMap =
        new()
        {
            { DateFormatCode.CC, ("yy", null) },
            { DateFormatCode.CD, ("MMMYYYY", null) },
            { DateFormatCode.CM, ("yyyyMM", null) },
            { DateFormatCode.CQ, ("yyyy'Q'q", null) },
            { DateFormatCode.CY, ("yyyy", null) },
            { DateFormatCode.D6, ("yyMMdd", null) },
            { DateFormatCode.D8, ("yyyyMMdd", null) },
            { DateFormatCode.DA, ("dd", "dd") },
            { DateFormatCode.DB, ("MMddyyyy", null) },
            { DateFormatCode.DD, ("dd", null) },
            { DateFormatCode.DDT, ("yyyyMMdd", "yyyyMMddHHmm") },
            { DateFormatCode.DT, ("yyyyMMddHHmm", null) },
            { DateFormatCode.DTD, ("yyyyMMddHHmm", "yyyyMMdd") },
            { DateFormatCode.DTS, ("yyyyMMddHHmmss", "yyyyMMddHHmmss") },
            { DateFormatCode.EH, ("yDDD", null) },
            { DateFormatCode.KA, ("yyMMMdd", null) },
            { DateFormatCode.MCY, ("MMyyyy", null) },
            { DateFormatCode.MD, ("MMdd", null) },
            { DateFormatCode.MM, ("MM", null) },
            { DateFormatCode.RD, ("MMddyyyy", "MMddyyyy") },
            { DateFormatCode.RD2, ("yy", "yy") },
            { DateFormatCode.RD4, ("yyyy", "yyyy") },
            { DateFormatCode.RD5, ("yyyyMM", "yyyyMM") },
            { DateFormatCode.RD6, ("yyMMdd", "yyMMdd") },
            { DateFormatCode.RD8, ("yyyyMMdd", "yyyyMMdd") },
            { DateFormatCode.RDM, ("yyMMdd", "MMdd") },
            { DateFormatCode.RDT, ("yyyyMMddHHmm", "yyyyMMddHHmm") },
            { DateFormatCode.RMD, ("MMdd", "MMdd") },
            { DateFormatCode.RMY, ("yyMM", "yyMM") },
            { DateFormatCode.RTM, ("HHmm", "HHmm") },
            { DateFormatCode.RTS, ("yyyyMMddHHmmss", null) },
            { DateFormatCode.TC, ("DDD", null) },
            { DateFormatCode.TM, ("HHmm", null) },
            { DateFormatCode.TQ, ("MMyy", null) },
            { DateFormatCode.TR, ("ddMMyyHHmm", null) },
            { DateFormatCode.TS, ("HHmmss", null) },
            { DateFormatCode.TT, ("MMddyy", null) },
            { DateFormatCode.TU, ("yyDDD", null) },
            { DateFormatCode.UN, ("", null) }, // Unstructured format
            { DateFormatCode.YM, ("yyMM", null) },
            { DateFormatCode.YMM, ("yyyyMMM", "MMM") },
            { DateFormatCode.YY, ("yy", null) }
        };

    public static (string, string?) GetFormat(DateFormatCode code)
    {
        if (_formatMap.TryGetValue(code, out var format)) return format;

        throw new ArgumentException($"No format string found for DateFormatCode: {code}");
    }

    public static DateFormatCode? GetCodeFromFormat(string format)
    {
        foreach (var kvp in _formatMap)
            if (kvp.Value.Item1 == format ||
                (kvp.Value.Item2 != null && $"{kvp.Value.Item1}-{kvp.Value.Item2}" == format))
                return kvp.Key;

        return null;
    }

    public static (DateTime, DateTime?) CreateDateTime(string dateString, string? endDateString = null)
    {
        DateTime startDate;
        DateTime? endDate = null;

        // Try parsing with all available formats
        foreach (var formatTuple in _formatMap.Values)
            if (TryParseDateTime(dateString, formatTuple.Item1, out startDate))
            {
                if (endDateString != null && formatTuple.Item2 != null)
                {
                    if (TryParseDateTime(endDateString, formatTuple.Item2, out var parsedEndDate))
                    {
                        endDate = parsedEndDate;
                    }
                    else
                    {
                        // If end date doesn't parse with the matching format, try parsing it with the start format
                        if (TryParseDateTime(endDateString, formatTuple.Item1, out parsedEndDate))
                            endDate = parsedEndDate;
                    }
                }

                return (startDate, endDate);
            }

        throw new FormatException($"Unable to parse the date string: {dateString}");
    }

    private static bool TryParseDateTime(string dateString, string format, out DateTime result)
    {
        return DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out result);
    }
}