using EdiSource.Domain.Loop;

namespace EdiSource.Domain.IO.EdiReader;

public interface IEdiParser<T> where T : class, ILoopInitialize<T>, new()
{
    Task<T> ParseEdi(FileInfo fileInfo);
    Task<T> ParseEdi(string ediText);
}