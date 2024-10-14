using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Parser;

public interface IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    /// <summary>
    ///     Converts a stream to a loop.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="separators">
    ///     If null, Separators.DefaultSeparators will be used,
    ///     or a value will be inferred is an Interchange Envelope
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Converts a file to a loop.
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="separators">
    ///     If null, Separators.DefaultSeparators will be used,
    ///     or a value will be inferred is an Interchange Envelope
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> ParseEdi(FileInfo fileInfo, Separators? separators = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Converts text to a loop.
    /// </summary>
    /// <param name="ediText"></param>
    /// <param name="separators">
    ///     If null, Separators.DefaultSeparators will be used,
    ///     or a value will be inferred is an Interchange Envelope
    /// </param>
    /// <returns></returns>
    Task<T> ParseEdi(string ediText, Separators? separators = null);
}