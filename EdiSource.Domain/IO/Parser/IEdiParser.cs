using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.IO.EdiReader;

public interface IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    Task<T> ParseEdi(StreamReader streamReader, Separators? separators = null);

    Task<T> ParseEdi(FileInfo fileInfo, Separators? separators = null);
    Task<T> ParseEdi(string ediText, Separators? separators = null);
    Task<T> ParseEdiEnvelope(StreamReader streamReader);
    Task<T> ParseEdiEnvelope(FileInfo fileInfo);
    Task<T> ParseEdiEnvelope(string ediText);
}