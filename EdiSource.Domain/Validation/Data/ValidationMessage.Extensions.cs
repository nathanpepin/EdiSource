namespace EdiSource.Domain.Validation.Data;

public static class ValidationMessageExtensions
{
    public static IEnumerable<ValidationMessage> UpdateSegmentLine(this IEnumerable<ValidationMessage> messages,
        int line)
    {
        var updateSegmentLine = messages as ValidationMessage[] ?? messages.ToArray();
        foreach (var message in updateSegmentLine)
        {
            message.SegmentLine = line;
        }

        return updateSegmentLine;
    }

    public static IEnumerable<ValidationMessage> UpdateLoopLine(this IEnumerable<ValidationMessage> messages,
        int line)
    {
        var updateSegmentLine = messages as ValidationMessage[] ?? messages.ToArray();
        foreach (var message in updateSegmentLine)
        {
            message.LoopLine = line;
        }

        return updateSegmentLine;
    }
}