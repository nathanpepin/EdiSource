namespace EdiSource.Domain.IO.EdiWriter;

public interface IEdiWriter : IDisposable
{
    IEdiWriter WriteText(string value);
    IEdiWriter WriteChar(char value);
    Task<IEdiWriter> WriteTextAsync(string value, CancellationToken cancellationToken = default);
    Task<IEdiWriter> WriteCharAsync(char value, CancellationToken cancellationToken = default);
    void Close();
    void Flush();
}