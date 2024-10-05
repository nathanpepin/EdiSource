using System.Text;

namespace EdiSource.Domain.IO.EdiWriter;

public sealed class StringBuilderEdiWriter(StringBuilder? stringBuilder = null) : IEdiWriter
{
    public StringBuilder Output { get; } = stringBuilder ?? new StringBuilder();

    public IEdiWriter WriteText(string value)
    {
        Output.Append(value);
        return this;
    }

    public IEdiWriter WriteChar(char value)
    {
        Output.Append(value);
        return this;
    }

    public Task<IEdiWriter> WriteTextAsync(string value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(WriteText(value));
    }

    public Task<IEdiWriter> WriteCharAsync(char value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(WriteChar(value));
    }

    public void Close()
    {
        Dispose();
    }

    public void Flush()
    {
    }

    public void Dispose()
    {
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }
}