namespace EdiSource.Domain.Validation.Data;

public class ValidationResult
{
    public bool IsValid => ValidationMessages.Any(x => x.ValidationSeverity <= ValidationSeverity.Info);
    public bool HasWarning => ValidationMessages.Any(x => x.ValidationSeverity >= ValidationSeverity.Warning);
    public bool HasError => ValidationMessages.Any(x => x.ValidationSeverity >= ValidationSeverity.Error);
    public bool HasCritical => ValidationMessages.Any(x => x.ValidationSeverity >= ValidationSeverity.Critical);

    public ValidationMessage[] WhereIsValid => ValidationMessages
        .Where(x => x.ValidationSeverity <= ValidationSeverity.Info)
        .ToArray();

    public ValidationMessage[] WhereHasWarning => ValidationMessages
        .Where(x => x.ValidationSeverity >= ValidationSeverity.Warning)
        .ToArray();

    public ValidationMessage[] WhereHasError => ValidationMessages
        .Where(x => x.ValidationSeverity >= ValidationSeverity.Error)
        .ToArray();

    public ValidationMessage[] WhereHasCritical => ValidationMessages
        .Where(x => x.ValidationSeverity >= ValidationSeverity.Critical)
        .ToArray();

    public List<ValidationMessage> ValidationMessages { get; } = [];

    public void AddRange(IEnumerable<ValidationMessage> updateSegmentLine)
    {
        ValidationMessages.AddRange(updateSegmentLine);
    }
}