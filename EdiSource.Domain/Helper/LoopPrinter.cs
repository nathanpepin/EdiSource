using System.Text;

namespace EdiSource.Domain.Helper;

public sealed class LoopPrinter
{
    private const string Indent =
        "                                                                                                                                ";

    private readonly StringBuilder _builder = new();
    private int _indentLevel;

    public void AppendLine(string line = "")
    {
        if (!string.IsNullOrEmpty(line))
        {
            _builder.Append(Indent.AsSpan(0, _indentLevel * 4));
            _builder.AppendLine(line);
        }
        else
        {
            _builder.AppendLine();
        }
    }


    public IDisposable AppendLoop(string line = "")
    {
        if (!string.IsNullOrEmpty(line))
        {
            _builder.Append(Indent[..(_indentLevel * 4)]);
            _builder.AppendLine(line);
        }
        else
        {
            _builder.AppendLine();
        }

        return new IndentationBlock(this);
    }

    public IDisposable IndentBlock()
    {
        return new IndentationBlock(this);
    }

    public override string ToString()
    {
        return _builder.ToString();
    }

    private class IndentationBlock : IDisposable
    {
        private readonly LoopPrinter _writer;

        public IndentationBlock(LoopPrinter writer)
        {
            _writer = writer;
            _writer._indentLevel++;
        }

        public void Dispose()
        {
            _writer._indentLevel--;
        }
    }
}