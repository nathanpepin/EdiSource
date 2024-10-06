using EdiSource.Domain.Loop;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public interface IEdiSerializer
{
    /// <summary>
    ///     Writes a loop to a stream
    /// </summary>
    /// <param name="loop"></param>
    /// <param name="stream"></param>
    /// <param name="separators">If null, the separators on the segment will be used</param>
    /// <param name="includeNewLine"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteToStream(ILoop loop, Stream stream, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default);


    /// <summary>
    ///     Writes a loop to a file
    /// </summary>
    /// <param name="loop"></param>
    /// <param name="fileInfo"></param>
    /// <param name="separators">If null, the separators on the segment will be used</param>
    /// <param name="includeNewLine"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteToFile(ILoop loop, FileInfo fileInfo, Separators? separators = null,
        bool includeNewLine = true, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Writes a loop to a string
    /// </summary>
    /// <param name="loop"></param>
    /// <param name="separators">If null, the separators on the segment will be used</param>
    /// <param name="includeNewLine"></param>
    /// <returns></returns>
    string WriteToString(ILoop loop, Separators? separators = null,
        bool includeNewLine = true);

    /// <summary>
    ///     Writes a loop out to a pretty string format.
    ///     Example (with '-' header instead of ' '):
    ///     <br />
    ///     <para>
    ///         TransactionSet<br />
    ///         ----ST*123<br />
    ///         ----DTP*01<br />
    ///         ----REF*02<br />
    ///         ----Loop1000<br />
    ///         --------NM1*12<br />
    ///         --------REF*3<br />
    ///     </para>
    /// </summary>
    /// <param name="loop"></param>
    /// <param name="separators">If null, the separators on the segment will be used</param>
    /// <returns></returns>
    string WriteToPrettyString(ILoop loop, Separators? separators = null);
}