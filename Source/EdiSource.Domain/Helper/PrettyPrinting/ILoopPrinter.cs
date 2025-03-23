namespace EdiSource.Domain.Helper.PrettyPrinting;

public interface ILoopPrinter
{
    /// <summary>
    ///     Appends a line with the current indentation
    /// </summary>
    void AppendLine();

    /// <summary>
    ///     Appends a line with the current indentation with the segment value
    /// </summary>
    /// <param name="segment"></param>
    void AppendLine(string segment);

    /// <summary>
    ///     Appends a line with the loopName and increase indentation
    /// </summary>
    /// <param name="loopName"></param>
    /// <returns></returns>
    IDisposable AppendLoop(string loopName);

    /// <summary>
    ///     Increases the indentation without appending a line
    /// </summary>
    /// <returns></returns>
    IDisposable IndentBlock();

    /// <summary>
    ///     Serializes the content to a string
    /// </summary>
    /// <returns></returns>
    string ToString();
}