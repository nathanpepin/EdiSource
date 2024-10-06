namespace EdiSource.Domain.Validation.Data;

/// <summary>
/// A helper class that holds validation messages
/// </summary>
public class EdiValidationResult
{
    public bool IsValid => ValidationMessages.Any(x => x.Severity <= ValidationSeverity.Info);
    public bool HasWarning => ValidationMessages.Any(x => x.Severity >= ValidationSeverity.Warning);
    public bool HasError => ValidationMessages.Any(x => x.Severity >= ValidationSeverity.Error);
    public bool HasCritical => ValidationMessages.Any(x => x.Severity >= ValidationSeverity.Critical);

    public ValidationMessage[] WhereIsValid => ValidationMessages
        .Where(x => x.Severity <= ValidationSeverity.Info)
        .ToArray();

    public ValidationMessage[] WhereHasWarning => ValidationMessages
        .Where(x => x.Severity >= ValidationSeverity.Warning)
        .ToArray();

    public ValidationMessage[] WhereHasError => ValidationMessages
        .Where(x => x.Severity >= ValidationSeverity.Error)
        .ToArray();

    public ValidationMessage[] WhereHasCritical => ValidationMessages
        .Where(x => x.Severity >= ValidationSeverity.Critical)
        .ToArray();

    public List<ValidationMessage> ValidationMessages { get; } = [];

    public void AddRange(IEnumerable<ValidationMessage> updateSegmentLine)
    {
        ValidationMessages.AddRange(updateSegmentLine);
    }
}