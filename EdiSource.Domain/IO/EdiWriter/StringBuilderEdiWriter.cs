using System.Text;

namespace EdiSource.Domain.IO.EdiWriter;

public class StringBuilderEdiWriter(StringBuilder? stringBuilder = null) : IEdiWriter
{
    public StringBuilder Output { get; } = stringBuilder ?? new StringBuilder();

    public IEdiWriter Write(string value)
    {
        Output.Append(value);
        return this;
    }

    public IEdiWriter Write(char value)
    {
        Output.Append(value);
        return this;
    }

    public Task<IEdiWriter> WriteText(string value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Write(value));
    }

    public Task<IEdiWriter> WriteAsync(char value, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Write(value));
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