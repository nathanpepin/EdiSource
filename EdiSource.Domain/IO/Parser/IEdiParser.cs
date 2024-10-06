using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Parser;

public interface IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    /// <summary>
    /// Converts a stream to a loop.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <returns></returns>
    Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null);

    /// <summary>
    /// Converts a file to a loop.
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <returns></returns>
    Task<T> ParseEdi(FileInfo fileInfo, Separators? separators = null);

    /// <summary>
    /// Converts text to a loop.
    /// </summary>
    /// <param name="ediText"></param>
    /// <param name="separators">If null, Separators.DefaultSeparators will be used</param>
    /// <returns></returns>
    Task<T> ParseEdi(string ediText, Separators? separators = null);

    /// <summary>
    /// Converts a stream to an interchange envelope.
    ///
    /// Separators not needed as they will be read from the ISA segment
    /// </summary>
    /// <param name="streamReader"></param>
    /// <returns></returns>
    Task<T> ParseEdiEnvelope(StreamReader streamReader);

    /// <summary>
    /// Converts a file to an interchange envelope.
    ///
    /// Separators not needed as they will be read from the ISA segment
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    Task<T> ParseEdiEnvelope(FileInfo fileInfo);

    /// <summary>
    /// Converts text to an interchange envelope.
    ///
    /// Separators not needed as they will be read from the ISA segment
    /// </summary>
    /// <param name="ediText"></param>
    /// <returns></returns>
    Task<T> ParseEdiEnvelope(string ediText);
}