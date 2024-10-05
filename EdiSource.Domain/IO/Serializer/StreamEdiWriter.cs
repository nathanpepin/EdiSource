namespace EdiSource.Domain.IO.Serializer;

public sealed class StreamEdiWriter(Stream stream) : IDisposable, IAsyncDisposable
{
    private readonly StreamWriter _output = new(stream);

    public async ValueTask DisposeAsync()
    {
        await _output.DisposeAsync();
        await stream.DisposeAsync();
    }

    public void Dispose()
    {
        _output.Dispose();
        stream.Dispose();
    }

    public StreamEdiWriter WriteText(string value)
    {
        _output.Write(value);
        return this;
    }

    public StreamEdiWriter WriteChar(char value)
    {
        _output.Write(value);
        return this;
    }

    public async Task<StreamEdiWriter> WriteTextAsync(string value, CancellationToken cancellationToken = default)
    {
        await _output.WriteAsync(value.ToCharArray());
        return this;
    }

    public async Task<StreamEdiWriter> WriteCharAsync(char value, CancellationToken cancellationToken = default)
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
}