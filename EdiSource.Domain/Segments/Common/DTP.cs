using System.Globalization;
using EdiSource.Domain.Segments.Extensions;

namespace EdiSource.Domain.Segments.Common;

public class DTP : Segment
{
    public string Qualifier
    {
        get => GetCompositeElement(1, 0);
        set => SetCompositeElement(1, 0, value);
    }

    public DateFormatCode DateFormatCode
    {
        get => this.GetEnumRequired<DateFormatCode>(2);
        set => this.SetEnum(value, 2);
    }

    public (string Date, string? DateRange) DateFormats => DateFormatMapper.GetFormat(DateFormatCode);

    public DateTime Date
    {
        get => this.GetDateRequired(3, format: DateFormats.Date);
        set => this.SetDate(value, 3, format: DateFormats.Date);
    }

    public DateTime? DateEnd
    {
        get => DateFormats.DateRange?
            .Map(format => this.GetDate(3, 1, format));
        set => DateFormats.DateRange?
            .Map(format => this.SetDate(value, 3, 1, format));
    }
}

public enum DateFormatCode
{
    CC, // First Two Digits of Year Expressed in Format CCYY
    CD, // Month and Year Expressed in Format MMMYYYY
    CM, // Date in Format CCYYMM
    CQ, // Date in Format CCYYQ
    CY, // Year Expressed in Format CCYY
    D6, // Date Expressed in Format YYMMDD
    D8, // Date Expressed in Format CCYYMMDD
    DA, // Range of Dates within a Single Month Expressed in Format DD-DD
    DB, // Date Expressed in Format MMDDCCYY
    DD, // Day of Month in Numeric Format
    DDT, // Range of Dates and Time, Expressed in CCYYMMDD-CCYYMMDDHHMM
    DT, // Date and Time Expressed in Format CCYYMMDDHHMM
    DTD, // Range of Dates and Time, Expressed in CCYYMMDDHHMM-CCYYMMDD
    DTS, // Range of Date and Time Expressed in Format CCYYMMDDHHMMSS-CCYYMMDDHHMMSS
    EH, // Last Digit of Year and Julian Date Expressed in Format YDDD
    KA, // Date Expressed in Format YYMMMDD
    MCY, // MMCCYY
    MD, // Month of Year and Day of Month Expressed in Format MMDD
    MM, // Month of Year in Numeric Format
    RD, // Range of Dates Expressed in Format MMDDCCYY-MMDDCCYY
    RD2, // Range of Years Expressed in Format YY-YY
    RD4, // Range of Years Expressed in Format CCYY-CCYY
    RD5, // Range of Years and Months Expressed in Format CCYYMM-CCYYMM
    RD6, // Range of Dates Expressed in Format YYMMDD-YYMMDD
    RD8, // Range of Dates Expressed in Format CCYYMMDD-CCYYMMDD
    RDM, // Range of Dates Expressed in Format YYMMDD-MMDD
    RDT, // Range of Date and Time, Expressed in Format CCYYMMDDHHMM-CCYYMMDDHHMM
    RMD, // Range of Months and Days Expressed in Format MMDD-MMDD
    RMY, // Range of Years and Months Expressed in Format YYMM-YYMM
    RTM, // Range of Time Expressed in Format HHMM-HHMM
    RTS, // Date and Time Expressed in Format CCYYMMDDHHMMSS
    TC, // Julian Date Expressed in Format DDD
    TM, // Time Expressed in Format HHMM
    TQ, // Date Expressed in Format MMYY
    TR, // Date and Time Expressed in Format DDMMYYHHMM
    TS, // Time Expressed in Format HHMMSS
    TT, // Date Expressed in Format MMDDYY
    TU, // Date Expressed in Format YYDDD
    UN, // Unstructured
    YM, // Year and Month Expressed in Format YYMM
    YMM, // Range of Year and Months, Expressed in CCYYMMM-MMM Format
    YY // Last Two Digits of Year Expressed in Format CCYY
}

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