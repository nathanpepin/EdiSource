namespace EdiSource.Domain.Validation.Data;

/// <summary>
/// Validation severities in escalating order
/// </summary>
public enum ValidationSeverity
{
    None = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}