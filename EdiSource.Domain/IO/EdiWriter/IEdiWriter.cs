namespace EdiSource.Domain.IO.EdiWriter;

public interface IEdiWriter : IDisposable
{
    IEdiWriter Write(string value);
    IEdiWriter Write(char value);
    Task<IEdiWriter> WriteText(string value, CancellationToken cancellationToken = default);
    Task<IEdiWriter> WriteAsync(char value, CancellationToken cancellationToken = default);
    void Close();
    void Flush();
}