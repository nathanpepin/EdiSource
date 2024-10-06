using EdiSource.Domain.Loop;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Factory;

public static partial class ValidationFactory
{
    public static ValidationMessage Create(ILoop loop, ValidationSeverity validationSeverity,
        string message) => new()
    {
        Severity = validationSeverity,
        Subject = "Loop",
        Message = message,
        Loop = loop.GetType().Name
    };

    public static ValidationMessage CreateCritical(ILoop loop,
        string message) => Create(loop, ValidationSeverity.Critical, message);

    public static ValidationMessage CreateError(ILoop loop,
        string message) => Create(loop, ValidationSeverity.Error, message);

    public static ValidationMessage CreateWarning(ILoop loop,
        string message) => Create(loop, ValidationSeverity.Warning, message);

    public static ValidationMessage CreateInfo(ILoop loop,
        string message) => Create(loop, ValidationSeverity.Info, message);
}