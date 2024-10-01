# EdiSource

A strongly-typed X12 EDI parsing, validation, and serialization library for .NET 9 using Roslyn source generators.

## Features

- **Source-generated models** -- Define EDI loops and segments with attributes; the Roslyn generator creates the parsing and initialization code at compile time.
- **Async streaming parser** -- Reads EDI documents via `System.Threading.Channels` for efficient, non-blocking processing.
- **Validation framework** -- Implements `IValidatable` on loops/segments to yield structured `ValidationMessage` results with severity levels.
- **Serialization** -- Write EDI structures back to strings, streams, or files.
- **X12 implementations included** -- 270 (Eligibility Inquiry) and 271 (Eligibility Response) transaction sets for 5010.

## Quick Start

### Parse an EDI envelope

```csharp
using EdiSource.Domain;

// From a string
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);

// From a file
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(new FileInfo("path/to/file.edi"));
```

### Parse a specific transaction set

```csharp
using EdiSource.Domain;

var result = await EdiCommon.ParseEdi<_270_5010_EligibilityBenefitInquiry>(ediText, separators);
```

### Validate

```csharp
var validationResult = EdiCommon.Validate(envelope);

foreach (var message in validationResult.ValidationMessages)
    Console.WriteLine($"[{message.Severity}] {message.Message}");
```

### Serialize back to EDI

```csharp
// To string
string ediOutput = EdiCommon.WriteEdiToString(envelope, separators);

// To file
await EdiCommon.WriteEdi(envelope, new FileInfo("output.edi"), separators);
```

## Defining Custom Transaction Sets

### 1. Define segments with attributes

```csharp
[SegmentGenerator<MyLoop>("ST")]
public sealed partial class MySegment
{
    public string TransactionSetIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}
```

The source generator automatically extends the class with `Segment` base class, `ISegmentIdentifier<MySegment>`, and `EdiId` matching.

### 2. Define loops with the LoopGenerator

```csharp
[LoopGenerator<ParentLoop, MyLoop, MyHeaderSegment>]
public sealed partial class MyLoop : IValidatable
{
    [SegmentHeader] public MyHeaderSegment Header { get; set; } = null!;
    [Segment] public MyDetailSegment? Detail { get; set; }
    [LoopList] public LoopList<ChildLoop> Children { get; set; } = [];
}
```

The source generator creates `InitializeAsync` methods that handle parsing from a `ChannelReader<Segment>`.

## Project Structure

```
EdiSource/
  Source/
    EdiSource.Domain/        Core library (parser, serializer, validation)
    EdiSource.Generator/     Roslyn source generators
    EdiSource.SampleApp/     Demo application (834 transaction)
  Tests/
    EdiSource.Domain.Tests/
    EdiSource.Generator.Tests/
    EdiSource.IntegrationTests/
  Implementation/
    270/                     Eligibility Inquiry (5010)
    271/                     Eligibility Response (5010)
```

## Building

```bash
dotnet build
dotnet test
```

## Requirements

- .NET 9.0 SDK
- C# 13 (preview)

## License

MIT
