namespace EdiSource.Domain.IO.EdiWriter;

public sealed class FileEdiWriter(string filePath) : IEdiWriter
{
    private readonly StreamEdiWriter _streamEdiWriter = new (new FileStream(filePath, FileMode.Create));

    public FileEdiWriter(FileSystemInfo fileSystemInfo) : this(fileSystemInfo.FullName)
    {
    }

    public IEdiWriter WriteText(string value)
    {
        _streamEdiWriter.WriteText(value);
        return this;
    }

    public IEdiWriter WriteChar(char value)
    {
        _streamEdiWriter.WriteChar(value);
        return this;
    }

    public async Task<IEdiWriter> WriteTextAsync(string value, CancellationToken cancellationToken = default)
    {
        await _streamEdiWriter.WriteTextAsync(value, cancellationToken);
        return this;
    }

    public async Task<IEdiWriter> WriteCharAsync(char value, CancellationToken cancellationToken = default)
    {
        await _streamEdiWriter.WriteCharAsync(value, cancellationToken);
        return this;
    }

    public void Close()
    {
        Dispose();
    }

    public void Flush()
    {
        _streamEdiWriter.Flush();
    }

    public void Dispose()
    {
        _streamEdiWriter.Dispose();
    }
}