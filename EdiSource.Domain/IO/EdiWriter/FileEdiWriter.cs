namespace EdiSource.Domain.IO.EdiWriter;

public class FileEdiWriter(string filePath) : IEdiWriter
{
    private readonly IEdiWriter _streamEdiWriter = new StreamEdiWriter(new FileStream(filePath, FileMode.Create));

    public FileEdiWriter(FileSystemInfo fileSystemInfo) : this(fileSystemInfo.FullName)
    {
    }

    public IEdiWriter Write(string value)
    {
        _streamEdiWriter.Write(value);
        return this;
    }

    public IEdiWriter Write(char value)
    {
        _streamEdiWriter.Write(value);
        return this;
    }

    public async Task<IEdiWriter> WriteText(string value, CancellationToken cancellationToken = default)
    {
        await _streamEdiWriter.WriteText(value, cancellationToken);
        return this;
    }

    public async Task<IEdiWriter> WriteAsync(char value, CancellationToken cancellationToken = default)
    {
        await _streamEdiWriter.WriteAsync(value, cancellationToken);
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