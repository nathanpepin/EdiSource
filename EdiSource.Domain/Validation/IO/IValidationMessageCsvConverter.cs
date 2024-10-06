using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.IO;

public interface IValidationMessageCsvConverter
{
    Task WriteToCsvAsync(ValidationResult result, FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    string ToCsvString(ValidationResult result);
}