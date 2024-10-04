using System;
using System.Collections.Generic;
using System.Text;

namespace EdiSource.Generator;

public sealed class CodeWriter
{
    private readonly StringBuilder _builder = new();
    private int _indentLevel;
    private readonly Stack<string> _disposeStack = new(0);

    private const string Indent =
        "                                                                                                                                ";

    public void AppendLine(string line = "")
    {
        if (!string.IsNullOrEmpty(line))
        {
            _builder.Append(Indent.Substring(0, _indentLevel * 4));
            _builder.AppendLine(line);
        }
        else
        {
            _builder.AppendLine();
        }
    }

    public void IncreaseIndent()
    {
        _indentLevel++;
    }

    public void DecreaseIndent()
    {
        _indentLevel--;
    }

    public IDisposable IndentBlock()
    {
        return new IndentationBlock(this);
    }

    private class IndentationBlock : IDisposable
    {
        private readonly CodeWriter _writer;

        public IndentationBlock(CodeWriter writer, string dispose = "}")
        {
            _writer = writer;
            _writer._indentLevel++;
            _writer._disposeStack.Push(dispose);
        }

        public void Dispose()
        {
            _writer._indentLevel--;
            _writer.AppendLine(_writer._disposeStack.Pop());
        }
    }

    public IDisposable StartNamespace(string namespaceName)
    {
        AppendLine($"namespace {namespaceName}");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public void AddUsing(string usingName)
    {
        AppendLine($"using {usingName};");
    }

    public IDisposable StartClass(string className, string modifier = "public", bool partial = true,
        string[]? implementations = null)
    {
        if (implementations is null)
        {
            AppendLine(partial
                ? $"{modifier} partial class {className}"
                : $"{modifier} class {className}");
            AppendLine("{");
            return new IndentationBlock(this);
        }

        AppendLine(partial
            ? $"{modifier} partial class {className} : {string.Join(", ", implementations)}"
            : $"{modifier} class {className}: {string.Join(", ", implementations)}");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public IDisposable StartMethod(string methodName, string returnType = "void", string modifier = "public",
        string[]? arguments = null)
    {
        AppendLine($"{modifier} {returnType} {methodName}({string.Join(", ", arguments ?? [])})");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public IDisposable StartConstructor(string className, string modifier = "public", params string[] arguments)
    {
        AppendLine($"{modifier} {className}({string.Join(", ", arguments)})");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public void AddAutoProperty(string propertyName, string propertyType, string modifier = "public",
        string? value = null)
    {
        if (value is not null)
            AppendLine($"{modifier} {propertyType} {propertyName} {{ get; set; }}");

        AppendLine($"{modifier} {propertyType} {propertyName} {{ get; set; }} = {value};");
    }

    public void AddCalcProperty(string propertyName, string propertyType, string calc, string modifier = "public")
    {
        AppendLine($"{modifier} {propertyType} {propertyName} => {calc};");
    }

    public IDisposable AddWhile(string condition)
    {
        AppendLine($"while ({condition})");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public IDisposable AddIf(string condition)
    {
        AppendLine($"if ({condition})");
        AppendLine("{");
        return new IndentationBlock(this);
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}