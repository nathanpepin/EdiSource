# EdiSource Library Architecture

A comprehensive technical reference for the internals of EdiSource -- a strongly-typed X12 EDI parsing, validation, and serialization library for .NET 9, powered by Roslyn source generators.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Core Domain Layer](#core-domain-layer)
  - [Elements](#elements)
  - [Segments](#segments)
  - [Loops](#loops)
  - [Envelope Structure](#envelope-structure)
  - [Separators](#separators)
  - [Identifiers and Pattern Matching](#identifiers-and-pattern-matching)
- [Source Generator Layer](#source-generator-layer)
  - [LoopGenerator](#loopgenerator)
  - [SegmentGenerator](#segmentgenerator)
  - [ValidationGenerator](#validationgenerator)
  - [Generated Code Anatomy](#generated-code-anatomy)
- [IO Layer](#io-layer)
  - [EdiReader](#edireader)
  - [EdiParser](#ediparser)
  - [EdiSerializer](#ediserializer)
  - [Streaming Pipeline](#streaming-pipeline)
- [Validation System](#validation-system)
  - [Validation Interfaces](#validation-interfaces)
  - [Source-Generated Validation Attributes](#source-generated-validation-attributes)
  - [ValidateEdi Walker](#validateedi-walker)
  - [Validation Results](#validation-results)
- [Error Handling](#error-handling)
  - [SegmentFactory and Near-Miss Detection](#segmentfactory-and-near-miss-detection)
  - [Exception Types](#exception-types)
- [Included Implementations](#included-implementations)
  - [270 Eligibility Inquiry (5010)](#270-eligibility-inquiry-5010)
  - [271 Eligibility Response (5010)](#271-eligibility-response-5010)
- [Areas for Improvement](#areas-for-improvement)

---

## Architecture Overview

EdiSource is built around a core principle: **define EDI structures declaratively with attributes, and let Roslyn source generators produce the parsing, serialization, and validation plumbing at compile time**.

The architecture has three layers:

```
+-------------------------------------------------------+
|              Source Generators (Compile-Time)           |
|   LoopGenerator  |  SegmentGenerator  |  ValidationGen |
+-------------------------------------------------------+
                          |
                    generates code for
                          |
+-------------------------------------------------------+
|                   Domain Layer (Runtime)                |
|  Elements | Segments | Loops | IO | Validation | IDs   |
+-------------------------------------------------------+
                          |
                    consumed by
                          |
+-------------------------------------------------------+
|              Implementation Layer (Per-Transaction)     |
|   270 (5010)  |  271 (5010)  |  834 (SampleApp)       |
+-------------------------------------------------------+
```

The streaming pipeline uses `System.Threading.Channels` for backpressure-aware, async-first parsing. The `EdiReader` produces raw `Segment` objects into a channel. Source-generated `InitializeAsync` methods on loop classes consume segments from the channel, constructing the typed object graph.

---

## Project Structure

```
EdiSource/
  Source/
    EdiSource.Domain/           Core library
      Elements/                 Element type (data element abstraction)
      Segments/                 ISegment, Segment base class, Extensions/
      Loop/                     LoopList, SegmentList, ILoop types, Extensions/
      IO/
        EdiReader/              Low-level streaming segment reader
        Parser/                 Channel-based async parser
        Serializer/             Write loops back to EDI format
      Identifiers/              EdiId, IEdi, ISegmentIdentifier, SegmentFactory
      Separator/                Separator detection (ISA-based)
      Validation/
        Data/                   ValidationMessage, EdiValidationResult types
        Factory/                ValidationFactory (Segment + Loop overloads)
        SourceGeneration/       13 validation attributes
        Validator/              ValidateEdi recursive walker
      Helper/                   Extension methods (enum parsing, general utils)
      Exceptions/               Custom exception types
      Standard/
        Loops/                  FunctionalGroup, ISA/InterchangeEnvelope
        Segments/               ISA, GS, GE, IEA segment definitions
    EdiSource.Generator/        Roslyn source generators (netstandard2.0)
    EdiSource.SampleApp/        834 demo application
  Implementation/
    270/EdiSource.270-5010/     270 Eligibility Inquiry transaction set
    271/EdiSource.271-5010/     271 Eligibility Response transaction set
  Tests/
    EdiSource.Domain.Tests/     Unit tests for domain
    EdiSource.Generator.Tests/  Snapshot tests for generated code
    EdiSource.IntegrationTests/ End-to-end parse/validate/serialize tests
```

### Project Dependencies

| Project | Target | References |
|---------|--------|------------|
| EdiSource.Domain | net9.0 | (none -- standalone) |
| EdiSource.Generator | netstandard2.0 | Microsoft.CodeAnalysis.CSharp 4.14 |
| EdiSource.270-5010 | net9.0 | Domain + Generator (as Analyzer) |
| EdiSource.271-5010 | net9.0 | Domain + Generator (as Analyzer) |
| EdiSource.SampleApp | net9.0 | Domain + Generator (as Analyzer) |

The Generator project targets `netstandard2.0` because Roslyn analyzers/generators must run in the compiler host, which requires this target.

---

## Core Domain Layer

### Elements

**File:** `Source/EdiSource.Domain/Elements/Element.cs`

`Element` represents a single data element, which internally holds a list of composite element values. Most data elements have a single composite value, so `Element` provides implicit conversions for convenience:

```csharp
// Implicit conversions
Element element = "20230101";           // string -> Element
Element element2 = ["A", "B", "C"];     // string[] -> Element
string value = element;                 // Element -> string (first composite)
string[] values = element2;             // Element -> string[]
```

`Element` implements `IList<string>` for full collection semantics. Key static methods:

- **`Element.FromString(segmentText, separators)`** -- Parses a segment string into an `Element[]`, splitting on data element and composite element separators.
- **`Element.MultipleFromString(segmentText, separators)`** -- Parses multiple segments into `Element[][]`.

The `Element` partial class also provides typed accessor extensions (defined via extension methods on `Segment`):

- `GetInt` / `GetIntRequired` / `SetInt` -- Integer parsing
- `GetDecimal` / `GetDecimalRequired` / `SetDecimal` -- Decimal parsing
- `GetDate` / `GetDateRequired` / `SetDate` -- DateTime parsing (format defaults to `"yyyyMMdd"`)
- `GetDateOnly` / `GetDateOnlyRequired` / `SetDateOnly` -- DateOnly parsing (format defaults to `"yyyyMMdd"`)
- `GetTimeOnly` / `GetTimeOnlyRequired` / `SetTimeOnly` -- TimeOnly parsing (format defaults to `"HHmm"`)
- `GetBool` / `GetBoolRequired` / `SetBool` -- Boolean parsing (requires `trueValue` parameter, e.g., `"Y"`)
- `GetEnum<T>` / `GetEnumRequired<T>` / `SetEnum<T>` -- Enum parsing from string values

### Segments

**File:** `Source/EdiSource.Domain/Segments/ISegment.cs`, `Segment.cs`

`Segment` is the base class for all EDI segments. It wraps `IList<Element>` (the data elements) and a `Separators` reference.

**`ISegment` interface provides:**

| Method | Description |
|--------|-------------|
| `this[int index]` | Get/set the first composite element at data element index |
| `this[int dataElement, int compositeElement]` | Get/set a specific composite element |
| `GetElement(int)` | Get element, throws if out of bounds |
| `GetElementOrNull(int)` | Get element or null |
| `GetCompositeElement(int, int)` | Get composite value, throws if not found |
| `GetCompositeElementOrNull(int, int)` | Get composite value or null |
| `SetDataElement(int, bool create, params string[])` | Set entire data element |
| `SetCompositeElement(string?, int, int, bool)` | Set a single composite value |
| `ElementExists(int)` | Check if data element exists |
| `CompositeElementExists(int, int)` | Check if composite element exists |
| `CompositeElementNotNullOrEmpty(int, int)` | Check if composite exists and has value |
| `Assign(Segment)` | Copy all elements from another segment |
| `Copy(Separators?, ILoop?)` | Deep clone the segment |

Segment properties in user-defined segment classes use `GetCompositeElement` / `SetCompositeElement` for element access, with indices being 0-based for the segment tag and 1-based for data:

```csharp
// Element 0 = segment tag ("NM1"), Element 1 = first data element
public string EntityIdentifierCode
{
    get => GetCompositeElement(1);
    set => SetCompositeElement(value, 1);
}
```

### Loops

Loops are the hierarchical containers in EDI. A loop has:

- **`[SegmentHeader]`** -- The first segment that identifies this loop (required for parsing)
- **`[Segment]`** -- Optional single segments
- **`[SegmentList]`** -- Optional repeating segments (`SegmentList<T>`)
- **`[Loop]`** -- Child loop (single)
- **`[LoopList]`** -- Child loop list (`LoopList<T>`)
- **`[SegmentFooter]`** -- Closing segment (e.g., SE, GE, IEA, LE)

**Key interfaces:**

- **`ILoop`** -- Marks a class as a loop; provides `EdiItems` (ordered list of all child EDI items)
- **`ILoopInitialize<T>`** -- Static abstract methods for constructing a loop from a `ChannelReader<Segment>`
- **`IEdi`** -- Base marker interface for all EDI types (Segment, SegmentList, Loop, LoopList)
- **`IEdi<T>`** -- Typed parent reference with `Parent` property and static `Validations` list

The `EdiItems` property is source-generated and returns all child properties in the correct order (header first, footer last). This enables the serializer and validator to walk the tree generically.

### Envelope Structure

The standard X12 envelope is modeled as:

```
InterchangeEnvelope (ILoop)
  ISA (Segment)
  FunctionalGroup[] (LoopList)
    GS (Segment)
    TransactionSet[] (LoopList -- via ITransactionSet)
    GE (Segment)
  IEA (Segment)
```

**Transaction set registration:** Transaction sets are registered dynamically via `InterchangeEnvelope.TransactionSetDefinitions`. Each implementation provides a `TransactionSetDefinition` that matches on the ST segment's identifier code and version:

```csharp
InterchangeEnvelope.TransactionSetDefinitions.Add(
    _270_5010_EligibilityBenefitInquiry.Definition
);
```

The `TransactionSetDefinition` is generated by `TransactionSetDefinitionsFactory<T>.CreateDefinition()` which produces a factory delegate that constructs the loop from a `ChannelReader<Segment>`.

### Separators

**File:** `Source/EdiSource.Domain/Separator/Separators.CreateISA.cs`

X12 EDI files encode their separators within the ISA segment itself. `Separators` auto-detects them:

- **Position 3** (after "ISA") -- Data element separator (typically `*`)
- **Position 104** -- Composite element separator (typically `:`)
- **Position 105** -- Segment separator (typically `~`)

```csharp
public static async Task<Separators> CreateFromISA(StreamReader reader)
```

Default separators: `*` (data), `:` (composite), `~` (segment).

### Identifiers and Pattern Matching

**`EdiId`** is a value type that represents an EDI segment's identity pattern. For example:

- `new EdiId("NM1")` -- matches any NM1 segment
- `new EdiId("NM1", "IL")` -- matches NM1 where element 1 is "IL"
- `new EdiId("NM1", "2B|36|GP|P5|PR")` -- matches NM1 where element 1 is any of those values
- `new EdiId("HL", null, null, "20")` -- matches HL where element 3 is "20"

The pipe `|` delimiter allows multi-value matching for a single element position. `null` elements are wildcards.

**`ISegmentIdentifier<T>`** provides:
- `static EdiId EdiId` -- The pattern for this segment type
- `static ValueTask<bool> MatchesAsync(ChannelReader<Segment>)` -- Peek at channel to check if next segment matches

---

## Source Generator Layer

Three Roslyn incremental generators run at compile time:

### LoopGenerator

**Triggered by:** `[LoopGenerator<TParent, TSelf, THeader>]` on a `partial class`

**Generates 3-4 files per loop:**

1. **`.EdiElement.g.cs`** -- `EdiItems` property that returns all child properties ordered: headers, segments, segment lists, loops, loop lists, footers.

2. **`.Implementation.g.cs`** -- Implements `ILoop`, `IEdi<TParent>`, `ISegmentIdentifier<TSelf>`:
   - `Parent` property
   - Static `EdiId` derived from the header segment's `EdiId`
   - `MatchesAsync` delegation to the header segment's matcher

3. **`.TransactionSet.g.cs`** (only if class implements `ITransactionSet<T>`) -- `TransactionSetDefinition` factory using `TransactionSetDefinitionsFactory<T>`.

4. **`.ChannelConstructor.g.cs`** -- The core parsing logic:
   - Empty constructor (for programmatic creation)
   - `ChannelReader<Segment>` constructor
   - `InitializeAsync(ChannelReader<Segment>, TParent?)` methods
   - The body is a `while (await segmentReader.WaitToReadAsync())` loop that peeks at each segment and dispatches to the correct property based on `ISegmentIdentifier<T>.MatchesAsync`.

**Property dispatch order in generated code:**
1. `[SegmentHeader]` -- Created via `SegmentLoopFactory<T, TLoop>.CreateAsync()`
2. `[Segment]` / `[SegmentList]` -- Matched and consumed in declaration order
3. `[Loop]` / `[LoopList]` -- Recursively initialized via `InitializeAsync`
4. `[SegmentFooter]` -- Consumed last
5. `[OptionalSegmentFooter]` -- Like footer but skipped if not present

### SegmentGenerator

**Triggered by:** `[SegmentGenerator<TParent>("TAG", ...)]` on a `partial class`

**Generates `.Implementation.g.cs`:**
- Extends the `Segment` base class
- Implements `IEdi<TParent>`, `ISegmentIdentifier<T>`
- `Parent` property
- Static `EdiId` built from the attribute parameters
- Constructor that copies element 0 values (the ID elements like "ST", "834")

The constructor type parameters:
- `TParent` -- The loop this segment belongs to
- First string arg -- Segment tag (e.g., "NM1", "ST", "HL")
- Additional string args -- Element 1 value, element 2 value, element 3 value for matching

### ValidationGenerator

**Triggered by:** `[LoopGeneratorAttribute<>]` combined with any validation attributes

**Generates** a `SourceGenValidations` property returning `IIndirectValidatable[]` -- one entry per validation attribute instance. This is consumed by the `ISourceGeneratorValidatable` interface during validation walking.

### Generated Code Anatomy

For a loop like:

```csharp
[LoopGenerator<FunctionalGroup, _834, TS_ST>]
public sealed partial class _834 : IValidatable, ITransactionSet<_834>
{
    [SegmentHeader] public TS_ST ST { get; set; } = default!;
    [SegmentList]   public SegmentList<TS_REF> REFs { get; set; } = [];
    [Segment]       public TS_DTP? DTP { get; set; }
    [Loop]          public Loop2000 Loop2000 { get; set; } = default!;
    [LoopList]      public LoopList<Loop2100> Loop2100s { get; set; } = [];
    [SegmentFooter] public TS_SE SE { get; set; } = default!;
}
```

The generator produces an `InitializeAsync` that:
1. Creates `ST` via `SegmentLoopFactory<TS_ST, _834>.CreateAsync(reader, parent)`
2. Enters a while loop, peeking at the channel
3. If next matches `TS_REF` -> read and add to `REFs`
4. If next matches `TS_DTP` -> read into `DTP`
5. If next matches `Loop2000` header -> recursively `Loop2000.InitializeAsync(reader, this)`
6. If next matches `Loop2100` header -> recursively `new Loop2100().InitializeAsync(reader, this)`, add to list
7. Falls through to `SE` footer creation
8. Returns self

---

## IO Layer

### EdiReader

**File:** `Source/EdiSource.Domain/IO/EdiReader/EdiReader.cs`

The lowest-level reader. Uses `ArrayPool<char>.Shared.Rent(4096)` for buffer management. Reads character by character from a `StreamReader`, splitting on separators:

- Segment separator (`~`) -> emit `Segment` to channel
- Data element separator (`*`) -> start new `Element`
- Composite element separator (`:`) -> add composite value to current `Element`
- `\r`, `\n`, `\0` -> ignored

Segments are emitted into a `Channel<Segment>` for consumption by downstream parsers.

**`ReadEdSegmentsIntoChannelAsync`** -- The core producer method. Reads the entire stream and writes each segment to the channel writer. Calls `channelWriter.Complete()` in the `finally` block. Note: the buffer loop iterates all `BufferSize` (4096) characters regardless of how many `ReadAsync` actually returned, which may process stale data on the final read.

**`ReadEdSegmentsAsync`** -- Convenience method that creates a bounded `Channel<Segment>(1)`, launches the producer, collects all segments into a `List<Segment>`, and returns them.

**`ReadBasicEdiAsync`** -- Returns a `BasicEdi` (segment array + separators) for simple use cases. Auto-detects separators from ISA.

### EdiParser

**File:** `Source/EdiSource.Domain/IO/Parser/EdiParser.cs`

`EdiParser<T>` orchestrates the full parsing pipeline:

1. Validates ISA if parsing an `InterchangeEnvelope`
2. Creates separators from ISA if needed
3. Creates an unbounded `Channel<Segment>`
4. Launches `EdiReader.ReadEdSegmentsIntoChannelAsync` as the producer
5. Launches `T.InitializeAsync(channel.Reader)` as the consumer
6. Awaits both via `Task.WhenAll`

The producer and consumer run concurrently -- as the reader produces segments, the generated loop initializer consumes them. The channel provides natural backpressure.

### EdiSerializer

Provides multiple serialization targets:

| Method | Description |
|--------|-------------|
| `WriteToString(loop, separators, includeNewLine)` | Returns EDI string |
| `WriteToStream(loop, stream, separators, ...)` | Writes to any `Stream` |
| `WriteToFile(loop, fileInfo, separators, ...)` | Writes to a file |
| `WriteToPrettyString(loop)` | Human-readable hierarchical format |

Serialization walks `loop.EdiItems` recursively, concatenating element values with the appropriate separators.

### Streaming Pipeline

The full parse pipeline:

```
StreamReader
    |
    v
EdiReader (4096-byte buffer, ArrayPool)
    |
    | Channel<Segment> (unbounded)
    v
Generated InitializeAsync (per loop type)
    |
    | Recursive construction
    v
Typed Object Graph (InterchangeEnvelope -> FunctionalGroup -> TransactionSet -> Loops)
```

This is fully asynchronous and streaming -- the reader does not need to finish before the parser starts consuming segments.

---

## Validation System

### Validation Interfaces

| Interface | Purpose | Implemented By |
|-----------|---------|----------------|
| `IValidatable` | Manual `Validate()` method returning `IEnumerable<ValidationMessage>` | User-defined loops/segments |
| `ISourceGeneratorValidatable` | `SourceGenValidations` property returning `IIndirectValidatable[]` | Generated from validation attributes |
| `IIndirectValidatable` | `Validate(IEdi)` method for attribute-based validation | Validation attributes |
| `IIndirectValidatable<T>` | Typed indirect validation delegate | Used by `IEdi<T>.Validations` |
| `IUserValidation<T>` | Static `UserValidations` list for external validation hooks | User-supplied delegates |

### Source-Generated Validation Attributes

These attributes are placed on segment classes and the `ValidationGenerator` emits code to instantiate them at compile time:

| Attribute | Description | Parameters |
|-----------|-------------|------------|
| `[ElementLength]` | Total composite element length within range | severity, dataElement, min, max |
| `[CompositeElementLength]` | Single composite element length within range | severity, dataElement, compositeElement, min, max |
| `[Empty]` | Element must be empty | severity, dataElement, compositeElement |
| `[NotEmpty]` | Element must not be empty | severity, dataElement, compositeElement |
| `[IsOneOfValues]` | Element must be one of specified values | severity, dataElement, compositeElement, params values |
| `[NotOneOfValues]` | Element must not be one of specified values | severity, dataElement, compositeElement, params values |
| `[RequiredDataElements]` | Specific data elements must exist | severity, params elementIndices |
| `[RequiredElement]` | Specific composite element must exist and be non-empty | severity, dataElement, compositeElement |
| `[BeDate]` | Element must parse as a valid date | severity, dataElement, compositeElement |
| `[BeDateTime]` | Element must parse as a valid datetime | severity, dataElement, compositeElement |
| `[BeTime]` | Element must parse as a valid time | severity, dataElement, compositeElement |
| `[BeInt]` | Element must parse as an integer | severity, dataElement, compositeElement |
| `[BeDecimal]` | Element must parse as a decimal | severity, dataElement, compositeElement |

All attributes take `ValidationSeverity` as the first parameter, which can be `Critical`, `Error`, `Warning`, or `Info`.

### ValidateEdi Walker

**File:** `Source/EdiSource.Domain/Validation/Validator/ValidateEdi.cs`

`ValidateEdi` is instantiated and its `Validate<T>(T ediItem)` instance method recursively walks the EDI tree. The `EdiCommon.Validate` facade wraps this as `new ValidateEdi().Validate(it)`:

1. **For `Segment`**: Runs `IValidatable.Validate()`, `ISourceGeneratorValidatable.SourceGenValidations`, and `IUserValidation<T>.UserValidations`. Increments segment line counter.
2. **For `IEnumerable<Segment>`** (SegmentList): Iterates and validates each segment.
3. **For `ILoop`**: Runs loop-level validations, then recurses into `loop.EdiItems`.
4. **For `IEnumerable<ILoop>`** (LoopList): Iterates and validates each loop.

Each `ValidationMessage` is annotated with `loopLine` and `segmentLine` for precise error location reporting.

### Validation Results

- **`ValidationMessage`** -- Contains: `Severity`, `Message`, `Subject` (Segment or Loop), `SegmentLine`, `LoopLine`, `Loop` (loop type name), `Segment` (segment data), `DataElement`, `CompositeElement`, `Value`
- **`EdiValidationResult`** -- Collection of `ValidationMessage` with `.IsValid`, `.ValidationMessages`, severity-filtered accessors (`.WhereHasWarning`, `.WhereHasError`, `.WhereHasCritical`)
- **`ValidationFactory`** -- Static factory: `ValidationFactory.Create(segment, severity, message, dataElement?)`
- **`ValidationMessageCsvConverter`** -- Exports validation results to CSV files

---

## Error Handling

### SegmentFactory and Near-Miss Detection

**File:** `Source/EdiSource.Domain/Identifiers/SegmentFactory.cs`

When a segment doesn't match during parsing, `SegmentLoopFactory<T, TLoop>` provides detailed diagnostic information:

1. **Near-miss detection** -- Walks up the loop hierarchy, collecting all known segment types in the same namespace via reflection. For each, it calculates a "match level" based on how many elements matched.

2. **Enhanced error messages** include:
   - The expected vs. received segment patterns
   - The parsing context (current loop, expected segment)
   - A ranked list of close matches with match levels
   - Suggested fixes (e.g., "Add element code 'IL' to existing NM1 segment")
   - Common troubleshooting steps

3. **Result caching** -- `ConcurrentDictionary<Type, List<SegmentInfo>>` caches segment type lookups per loop type for performance.

### Exception Types

| Exception | When Thrown |
|-----------|------------|
| `InvalidISAException` | ISA segment is missing, too short, or stream is not seekable |
| `EdiReaderException` | No segments could be read from the stream |
| `EmptySegmentException` | An empty segment (consecutive segment separators) was encountered |
| `EdiParsingException` | Segment mismatch during structured parsing (includes near-miss info) |
| `DataElementParsingError` | Element could not be parsed as the expected type (int, date, etc.) |

---

## Included Implementations

### 270 Eligibility Inquiry (5010)

**Project:** `Implementation/270/EdiSource.270-5010/`

Implements the ANSI X12 270 (Health Care Eligibility/Benefit Inquiry) 5010 transaction set:

```
_270_5010_EligibilityBenefitInquiry (Transaction Set)
  ST (Header)
  BHT (Beginning of Hierarchical Transaction)
  Loop2000A (Information Source)
    HL (Hierarchical Level, code "20")
    Loop1000A (Source Name)
      NM1 (Entity codes: 2B|36|GP|P5|PR)
  Loop2000B (Information Receiver)
    HL (Hierarchical Level, code "21")
    Loop1000B (Receiver Name)
      NM1, REF[], N3, N4, PRV
  Loop2000C (Subscriber)
    HL (Hierarchical Level, code "22")
    TRN[] (Trace Numbers, max 2)
    Loop2100C (Subscriber Name)
      NM1 (Entity code: IL), REF[], N3, N4, PRV, DMG, INS, HI, DTP[]
    Loop2110C[] (Eligibility/Benefit Inquiry)
      EQ, AMT (SpendDown), AMT (TotalBilled), III, REF, DTP
  Loop2000D (Dependent)
    HL (Hierarchical Level, code "23")
    TRN[] (max 2)
    Loop2100D (Dependent Name)
      NM1 (Entity code: 03), REF[], N3, N4, PRV, DMG, INS, HI, DTP[]
    Loop2110D[] (Dependent Inquiry)
      EQ, III[], REF[], DTP[]
  SE (Trailer)
```

### 271 Eligibility Response (5010)

**Project:** `Implementation/271/EdiSource.271-5010/`

Implements the ANSI X12 271 (Health Care Eligibility/Benefit Response) 5010 transaction set. Notable differences from 270:

- **EB segments** (Eligibility/Benefit Information) replace EQ
- **HSD segments** (Health Care Services Delivery) for service details
- **AAA segments** (Request Validation) for error/rejection reporting
- **MSG segments** (Free-form text messages)
- **LS/LE grouping** for Loop2120C/2120D (Benefit Related Entity) -- uses `[SegmentHeader]` on LS and `[OptionalSegmentFooter]` on LE to handle the grouping pattern
- **MP segments** (Military Personnel Information)
- Deeper loop nesting with response-specific data

---

## Areas for Improvement

### Performance
- **EdiReader buffer handling**: The current implementation reads into a rented 4096-char buffer but processes character-by-character. Consider using `ReadOnlySpan<char>` and `MemoryExtensions` for SIMD-friendly scanning of separator characters. The `for` loop iterates the full buffer size regardless of actual bytes read (the `ReadAsync` return value is unused).
- **StringBuilder allocations**: Each segment field accumulates via `StringBuilder`. For small fields (most EDI elements are short), `stackalloc` + `Span<char>` could avoid heap allocations.
- **SegmentFactory reflection**: Near-miss detection uses `Assembly.GetTypes()` and reflection over `ISegmentIdentifier<>` implementations. While cached, the initial scan could be slow for large assemblies. Consider source-generating a segment registry instead.
- **Channel bounded vs. unbounded**: `EdiReader.ReadEdSegmentsAsync` uses `Channel.CreateBounded<Segment>(1)` while `EdiParser` uses `Channel.CreateUnbounded<Segment>()`. The bounded channel serializes reader/consumer but could be tuned for throughput.

### API Design
- **`EdiCommon` static API**: The facade class mixes concerns (parsing, serialization, validation, CSV export). Consider splitting into `EdiParser`, `EdiSerializer`, `EdiValidator` top-level APIs, or at least namespacing the methods.
- **Nullable property patterns**: Some segment properties return `string` (throwing on null) while others return `string?`. A more consistent approach to required vs. optional elements would help users.
- **Element index confusion**: Element indices are 0-based where index 0 is the segment tag. The first *data* element is index 1. This is correct per X12 but could benefit from XML doc comments or a named constant pattern.
- **`GetCompositeElement` vs `this[int]`**: Both exist for element access. The indexer and the method should be clearly documented as equivalent or differentiated.

### Validation
- **No cross-segment validation framework**: Validating that SE segment count matches actual segment count, or that HL parent IDs form a valid hierarchy, requires manual `IValidatable` code. A built-in cross-reference validation pattern would reduce boilerplate.
- **Validation attribute discoverability**: The 13 attributes are powerful but lack IDE discoverability. A Roslyn analyzer that suggests missing validations based on X12 specs could help.
- **Validation result locators**: `LoopLine` and `SegmentLine` are integer counters. Adding the actual segment tag and loop type name would make validation messages more useful without requiring the original EDI text.

### Source Generators
- **No incremental caching**: The generators use `ForAttributeWithMetadataName` (good) but the transform pipeline could cache more aggressively to avoid re-generation on non-EDI file changes.
- **Error diagnostics**: Generator errors silently produce no output. Emitting Roslyn diagnostics (warnings/errors) when attribute parameters are invalid would improve the developer experience.
- **Multi-value EdiId syntax**: The pipe separator (`|`) for multi-value matching in `EdiId` is parsed at runtime. This could be validated at compile time by the generator.

### Testing
- **Integration test coverage**: The 270/271 test projects each have a single test. More edge-case tests (missing optional segments, repeated segments at max counts, malformed data) would improve confidence.
- **Property-based testing**: The parsing/serialization roundtrip is a natural candidate for property-based testing (generate random valid EDI, parse, serialize, verify equality).
- **Benchmark suite**: No benchmarks exist for the streaming pipeline. Adding BenchmarkDotNet tests would help track performance regressions.

### Documentation
- **XML documentation coverage**: Many internal classes lack XML doc comments. The public API (`EdiCommon`, `ISegment`, `Element`) has good docs but generated code does not.
- **X12 spec references**: Linking segment/element definitions to the relevant X12 specification sections would help implementers.

### Architecture
- **No EDIFACT support**: The library is X12-only. The `Separators` and `EdiReader` are tightly coupled to X12 syntax. An abstraction layer for different EDI standards would enable EDIFACT or HL7 support.
- **No segment repetition limits**: X12 specs define max repetitions for segments/loops (e.g., "max 9 REF segments"). While some implementations validate this in `IValidatable`, there's no declarative way to express it.
- **No streaming validation**: Validation requires the full object graph. A streaming validator that validates segments as they arrive (before the full tree is built) would catch errors earlier.
- **Read-only parsing mode**: There's no way to parse an EDI document into a read-only structure. All parsed objects are mutable, which could lead to accidental modification.
