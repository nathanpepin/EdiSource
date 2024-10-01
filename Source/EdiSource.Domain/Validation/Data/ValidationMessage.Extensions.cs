namespace EdiSource.Domain.Validation.Data;

internal static class ValidationMessageExtensions
{
    /// <summary>
    ///     Updates all validation message's SegmentLine to the input value
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static IEnumerable<ValidationMessage> UpdateSegmentLine(this IEnumerable<ValidationMessage> messages,
        int line)
    {
        var updateSegmentLine = messages as ValidationMessage[] ?? messages.ToArray();
        foreach (var message in updateSegmentLine) message.SegmentLine = line;

        return updateSegmentLine;
    }

    /// <summary>
    ///     Updates all validation message's LoopLine to the input value
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static IEnumerable<ValidationMessage> UpdateLoopLine(this IEnumerable<ValidationMessage> messages,
        int line)
    {
        var updateSegmentLine = messages as ValidationMessage[] ?? messages.ToArray();
        foreach (var message in updateSegmentLine) message.LoopLine = line;

        return updateSegmentLine;
    }
}