using System.Text;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.IO;

public class ValidationMessageCsvConverter : IValidationMessageCsvConverter
{
    public async Task WriteToCsvAsync(ValidationResult result, FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        await File.WriteAllTextAsync(fileInfo.FullName, ToCsvString(result), cancellationToken);
    }

    public string ToCsvString(ValidationResult result)
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine(GetCsvHeader());

        foreach (var message in result.ValidationMessages)
        {
            csvBuilder.AppendLine(ConvertToCsvRow(message));
        }

        return csvBuilder.ToString();
    }

    private static string GetCsvHeader()
    {
        return string.Join(",", [
            "ValidationSeverity",
            "Message",
            "Subject",
            "LoopLine",
            "SegmentLine",
            "Loop",
            "Segment",
            "DataElement",
            "CompositeElement"
        ]);
    }

    private static string ConvertToCsvRow(ValidationMessage message)
    {
        return string.Join(",", new[]
        {
            EscapeCsvField(message.ValidationSeverity.ToString()),
            EscapeCsvField(message.Message),
            EscapeCsvField(message.Subject),
            EscapeCsvField(message.LoopLine?.ToString() ?? ""),
            EscapeCsvField(message.SegmentLine?.ToString() ?? ""),
            EscapeCsvField(message.Loop ?? ""),
            EscapeCsvField(message.Segment ?? ""),
            EscapeCsvField(message.DataElement?.ToString() ?? ""),
            EscapeCsvField(message.CompositeElement?.ToString() ?? "")
        });
    }

    private static string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return "";

        var needsQuotes = field.Contains(',') || field.Contains('"') || field.Contains('\r') || field.Contains('\n');

        return needsQuotes ? $"\"{field.Replace("\"", "\"\"")}\"" : field;
    }
}