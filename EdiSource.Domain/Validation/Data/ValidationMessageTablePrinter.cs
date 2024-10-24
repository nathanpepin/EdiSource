namespace EdiSource.Domain.Validation.Data;

public static class ValidationMessageTablePrinter
{
    private const int SeverityWidth = 10;
    private const int SubjectWidth = 20;
    private const int LoopWidth = 15;
    private const int SegmentWidth = 70;
    private const int DataElementWidth = 15;
    private const int ValueWidth = 20;
    private const int MessageWidth = 60;

    private static string _separator = string.Empty;

    public static void PrintColorCodedValidationMessagesTable(EdiValidationResult validationResult,
        params ValidationSeverity[] severities)
    {
        var messages = validationResult
            .ValidationMessages
            .Where(x => severities.Contains(x.Severity));

        PrintColorCodedValidationMessagesTable([..messages]);
    }

    public static void PrintColorCodedValidationMessagesTable(IEnumerable<ValidationMessage> messages)
    {
        PrintColorCodedValidationMessagesTable([..messages]);
    }

    public static void PrintColorCodedValidationMessagesTable(ValidationMessage[] messages)
    {
        if (messages.Length == 0)
        {
            Console.WriteLine("No validation messages to display.");
            return;
        }

        PrintHeader();

        foreach (var message in messages) PrintRow(message);

        PrintSeparator();
    }

    private static void PrintHeader(params int[] columnWidths)
    {
        PrintSeparator();
        Console.ForegroundColor = ConsoleColor.Cyan;

        var headerText = $"| {"Severity".Adjust(SeverityWidth)} " +
                         $"| {"Subject".Adjust(SubjectWidth)} " +
                         $"| {"Loop".Adjust(LoopWidth)} " +
                         $"| {"Data Element".Adjust(DataElementWidth)} " +
                         $"| {"Value".Adjust(ValueWidth)} " +
                         $"| {"Message".Adjust(MessageWidth)} " +
                         $"| {"Segment".Adjust(SegmentWidth)} |";

        if (_separator.Length == 0) _separator = new string('-', headerText.Length);

        Console.WriteLine(headerText);
        Console.ResetColor();
        PrintSeparator();
    }

    private static void PrintRow(ValidationMessage message, params int[] columnWidths)
    {
        SetSeverityColor(message.Severity);
        Console.Write($"| {message.Severity.ToString().Adjust(SeverityWidth)} ");
        Console.ResetColor();
        Console.WriteLine(
            $"| {message.Subject.ToString().Adjust(SubjectWidth)} " +
            $"| {(message.Loop ?? "").Adjust(LoopWidth)} " +
            $"| {(message.DataElement.HasValue ? message.DataElement.Value.ToString() : "").Adjust(DataElementWidth)} " +
            $"| {(message.Value ?? "").Adjust(ValueWidth)} " +
            $"| {message.Message.Adjust(MessageWidth)} " +
            $"| {(message.Segment ?? "").Adjust(SegmentWidth)} |"
        );
    }

    private static string Adjust(this string value, int width)
    {
        var needsTrimming = value.Length > width;

        if (!needsTrimming) return value.PadRight(width, ' ');

        return value[..(width - 3)] + "...";
    }

    private static void PrintSeparator()
    {
        Console.WriteLine(_separator);
    }

    private static void SetSeverityColor(ValidationSeverity severity)
    {
        Console.ForegroundColor = severity switch
        {
            ValidationSeverity.Critical => ConsoleColor.Magenta,
            ValidationSeverity.Error => ConsoleColor.Red,
            ValidationSeverity.Warning => ConsoleColor.Yellow,
            ValidationSeverity.Info => ConsoleColor.Green,
            _ => ConsoleColor.Gray
        };
    }
}