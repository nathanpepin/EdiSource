using EdiSource.Domain.Identifiers;
using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.IO.Serializer;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.IO;
using EdiSource.Domain.Validation.Validator;

namespace EdiSource.Domain;

/// <summary>
///     Common functionality for most use cases
/// </summary>
public static class EdiCommon
{
    /// <summary>
    ///     Parses an EDI envelope from a StreamReader.
    /// </summary>
    /// <typeparam name="T">The type of the object that implements ILoopInitialize.</typeparam>
    /// <param name="stream">The StreamReader to read the EDI envelope from.</param>
    /// <param name="separators">The edi separators, or the default ones from Separators.DefaultSeparators if not provided</param>
    /// <param name="cancellationToken">Optional. A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous parse operation. The task result contains the parsed object.</returns>
    public static Task<T> ParseEdi<T>(StreamReader stream, Separators? separators = null,
        CancellationToken cancellationToken = default)
        where T : class, ILoopInitialize<T>, new()
    {
        return new EdiParser<T>().ParseEdi(stream, separators, cancellationToken);
    }

    /// <summary>
    ///     Parses an EDI envelope from the given FileInfo with an optional cancellation token.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the data structure to parse the EDI envelope into. Must implement ILoopInitialize&lt;T
    ///     &gt; and have a parameterless constructor.
    /// </typeparam>
    /// <param name="fileInfo">The FileInfo from which to read the EDI data.</param>
    /// <param name="separators">The edi separators, or the default ones from Separators.DefaultSeparators if not provided</param>
    /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous parse operation, with the parsed EDI data structure as its result.</returns>
    public static Task<T> ParseEdi<T>(FileInfo fileInfo, Separators? separators = null,
        CancellationToken cancellationToken = default)
        where T : class, ILoopInitialize<T>, new()
    {
        return new EdiParser<T>().ParseEdi(fileInfo, separators, cancellationToken);
    }

    /// <summary>
    ///     Parses an EDI envelope from a stream and returns an object of type T,
    ///     which must be a class implementing ILoopInitialize.
    /// </summary>
    /// <typeparam name="T">The type of object to return, which must implement ILoopInitialize.</typeparam>
    /// <param name="text">The text to read the EDI envelope data from.</param>
    /// <param name="separators">The edi separators, or the default ones from Separators.DefaultSeparators if not provided</param>
    /// <returns>A task representing the asynchronous operation, with a result of type T.</returns>
    public static Task<T> ParseEdi<T>(string text, Separators? separators = null)
        where T : class, ILoopInitialize<T>, new()
    {
        return new EdiParser<T>().ParseEdi(text, separators);
    }

    /// <summary>
    ///     Writes an EDI representation of the given loop to the specified stream.
    /// </summary>
    /// <typeparam name="T">The type of the EDI loop.</typeparam>
    /// <param name="loop">The EDI loop to be written.</param>
    /// <param name="stream">The stream to which the EDI data will be written.</param>
    /// <param name="separators">
    ///     Optional separators to use for delimiting the EDI data. If null, default separators will be
    ///     used.
    /// </param>
    /// <param name="includeNewLine">Indicates whether to include new lines in the EDI output.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    public static Task WriteEdi<T>(T loop, Stream stream, Separators? separators = null, bool includeNewLine = true,
        CancellationToken cancellationToken = default)
        where T : class, ILoop
    {
        return new EdiSerializer().WriteToStream(loop, stream, separators, includeNewLine, cancellationToken);
    }

    /// <summary>
    ///     Writes EDI data to a specified file.
    /// </summary>
    /// <param name="loop">The loop containing the EDI data to write.</param>
    /// <param name="fileInfo">The file information specifying the file to write to.</param>
    /// <param name="separators">Optional. The separators to use in the EDI data.</param>
    /// <param name="includeNewLine">Indicates whether to include new lines in the EDI output.</param>
    /// <param name="cancellationToken">Optional. The cancellation token to cancel the operation.</param>
    /// <typeparam name="T">The type of the loop.</typeparam>
    /// <returns>A task representing the asynchronous write operation.</returns>
    public static Task WriteEdi<T>(T loop, FileInfo fileInfo, Separators? separators = null, bool includeNewLine = true,
        CancellationToken cancellationToken = default)
        where T : class, ILoop
    {
        return new EdiSerializer().WriteToFile(loop, fileInfo, separators, includeNewLine, cancellationToken);
    }

    /// <summary>
    ///     Converts the provided loop structure to an EDI string format.
    /// </summary>
    /// <param name="loop">The loop structure to convert to an EDI string.</param>
    /// <param name="separators">
    ///     The optional separators to use for the EDI string. If not provided, default separators will be
    ///     used.
    /// </param>
    /// <param name="includeNewLine">Specifies whether to include newline characters in the output EDI string.</param>
    /// <returns>A string representing the loop structure in EDI format.</returns>
    public static string WriteEdiToString<T>(T loop, Separators? separators = null, bool includeNewLine = true)
        where T : class, ILoop
    {
        separators ??= Separators.DefaultSeparators;
        return new EdiSerializer().WriteToString(loop, separators, includeNewLine);
    }

    /// <summary>
    ///     Validates the provided EDI item and returns the validation results.
    /// </summary>
    /// <typeparam name="T">The type of the EDI item to be validated, which must implement the IEdi interface.</typeparam>
    /// <param name="it">The instance of the EDI item to be validated.</param>
    /// <returns>An EdiValidationResult instance containing the results of the validation.</returns>
    public static EdiValidationResult Validate<T>(T it)
        where T : IEdi
    {
        var validator = new ValidateEdi();
        return validator.Validate(it);
    }

    /// <summary>
    ///     Writes the validation messages to a CSV file.
    /// </summary>
    /// <param name="validationResult">The EDI validation result containing validation messages.</param>
    /// <param name="fileInfo">The FileInfo object representing the file to which the CSV will be written.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static Task WriteValidationsToCsvFile(EdiValidationResult validationResult, FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        var writer = new ValidationMessageCsvConverter();
        return writer.WriteToCsvAsync(validationResult, fileInfo, cancellationToken);
    }

    /// <summary>
    ///     Generates a human-readable representation of the EDI structure.
    /// </summary>
    /// <typeparam name="T">The type of the EDI loop implementing ILoop.</typeparam>
    /// <param name="it">The instance of the EDI loop to be pretty printed.</param>
    /// <returns>A string that contains the pretty-printed representation of the EDI structure.</returns>
    public static string PrettyPrint<T>(T it)
        where T : ILoop
    {
        return new EdiSerializer().WriteToPrettyString(it);
    }
}