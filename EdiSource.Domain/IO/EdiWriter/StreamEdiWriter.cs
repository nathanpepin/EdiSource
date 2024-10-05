namespace EdiSource.Domain.IO.EdiWriter;

public class StreamEdiWriter(Stream stream) : IEdiWriter
{
    private readonly StreamWriter _output = new(stream);

    public IEdiWriter WriteText(string value)
    {
        _output.Write(value);
        return this;
    }

    public IEdiWriter WriteChar(char value)
    {
        _output.Write(value);
        return this;
    }

    public async Task<IEdiWriter> WriteTextAsync(string value, CancellationToken cancellationToken = default)
    {
        await _output.WriteAsync(value.ToCharArray());
        return this;
    }

    public async Task<IEdiWriter> WriteCharAsync(char value, CancellationToken cancellationToken = default)
    {
        await _output.WriteAsync(new[] { value });
        return this;
    }

    public void Flush()
    {
        _output.Flush();
    }

    public void Close()
    {
        Dispose();
    }

    public void Dispose()
    {
        _output.Dispose();
    }
}