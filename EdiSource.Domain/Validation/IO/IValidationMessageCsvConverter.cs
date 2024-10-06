using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.IO;

/// <summary>
///     Converts validation messages to a comma seperated value
/// </summary>
public interface IValidationMessageCsvConverter
{
    Task WriteToCsvAsync(EdiValidationResult result, FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    string ToCsvString(EdiValidationResult result);
}