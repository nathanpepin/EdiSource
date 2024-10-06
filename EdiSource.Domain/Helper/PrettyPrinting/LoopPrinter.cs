using System.Text;

namespace EdiSource.Domain.Helper.PrettyPrinting;

/// <summary>
/// Used for pretty printing loops and segments
/// </summary>
public sealed class LoopPrinter : ILoopPrinter
{
    private const string Indent =
        "                                                                                                                                ";

    private readonly StringBuilder _builder = new();
    private int _indentLevel;

    public void AppendLine()
    {
        _builder.AppendLine();
    }

    public void AppendLine(string segment)
    {
        _builder.Append(Indent.AsSpan(0, _indentLevel * 4));
        _builder.AppendLine(segment);
    }


    public IDisposable AppendLoop(string loopName)
    {
        if (!string.IsNullOrEmpty(loopName))
        {
            _builder.Append(Indent[..(_indentLevel * 4)]);
            _builder.AppendLine(loopName);
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