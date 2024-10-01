# EdiSource AI Agent Reference

A structured reference for AI coding agents working with the EdiSource codebase. This document provides the patterns, conventions, file locations, and decision framework needed to understand, modify, and extend the library.

---

## Table of Contents

- [Quick Orientation](#quick-orientation)
- [Critical Patterns to Follow](#critical-patterns-to-follow)
- [Project Map](#project-map)
- [File Location Index](#file-location-index)
- [How Parsing Works (End-to-End)](#how-parsing-works-end-to-end)
- [How to Add a New Transaction Set](#how-to-add-a-new-transaction-set)
- [How to Add a New Segment](#how-to-add-a-new-segment)
- [How to Add a New Loop](#how-to-add-a-new-loop)
- [How to Add Validation](#how-to-add-validation)
- [Source Generator Patterns](#source-generator-patterns)
- [Naming Conventions](#naming-conventions)
- [Common Pitfalls](#common-pitfalls)
- [Testing Patterns](#testing-patterns)
- [Dependency Graph](#dependency-graph)
- [Key Interfaces and What They Do](#key-interfaces-and-what-they-do)
- [Element Indexing Rules](#element-indexing-rules)
- [EdiId Pattern Matching Rules](#ediid-pattern-matching-rules)
- [Validation System Layers](#validation-system-layers)
- [Envelope Structure](#envelope-structure)
- [Areas for Improvement](#areas-for-improvement)

---

## Quick Orientation

**What is this?** A .NET 9 library for parsing, validating, and serializing X12 EDI documents. Uses Roslyn source generators to auto-generate parsing code from attribute-decorated partial classes.

**Language:** C# 13 (preview), .NET 9.0, nullable enabled, warnings as errors.

**Core idea:** Users define `partial class` segment/loop types with attributes. Source generators produce the `InitializeAsync` methods, `EdiItems` properties, `ISegmentIdentifier` implementations, and validation runners. At runtime, `System.Threading.Channels` stream raw segments from the reader to the generated constructors.

**Entry point for users:** `EdiCommon` static class in `Source/EdiSource.Domain/EdiCommon.cs`.

**Build:** `dotnet build` from repo root. **Test:** `dotnet test` from repo root.

---

## Critical Patterns to Follow

When modifying or extending this codebase, these are non-negotiable:

1. **All loop and segment classes must be `partial`.** The source generators extend them with generated files. Removing `partial` will break compilation.

2. **Property declaration order in loops determines parse order.** The generator processes `[SegmentHeader]`, `[Segment]`, `[SegmentList]`, `[Loop]`, `[LoopList]`, `[SegmentFooter]` in the order they appear in the class. Changing the order changes parsing behavior.

3. **`SegmentGenerator<TParent>` -- the generic parameter is the parent loop, not the segment itself.** This is a common confusion. The segment belongs to `TParent`.

4. **`LoopGenerator<TParent, TSelf, THeader>` -- three type parameters.** Parent loop, self type, and header segment type.

5. **Element index 0 is the segment tag.** The first *data* element is index 1. Always use 1-based indexing for data elements.

6. **The Generator project targets `netstandard2.0`.** This is a Roslyn requirement. Do not change this. Do not use modern C# features (like file-scoped namespaces, records, nullable annotations) in the Generator project.

7. **`InterchangeEnvelope.TransactionSetDefinitions` must be populated before parsing.** Transaction sets won't be recognized without registration.

---

## Project Map

```
EdiSource.sln
├── Source/
│   ├── EdiSource.Domain/           # Core runtime library (THE main project)
│   │   ├── EdiCommon.cs            # Public API facade
│   │   ├── Assembly.cs             # InternalsVisibleTo declarations
│   │   ├── EdiSource.Domain.csproj # net9.0, MIT license, NuGet metadata
│   │   ├── Elements/               # Element type (data element container)
│   │   ├── Segments/               # ISegment, Segment base class, Extensions/
│   │   ├── Loop/                   # LoopList, SegmentList, ILoop, Extensions/
│   │   ├── IO/
│   │   │   ├── EdiReader/          # Low-level char-by-char streaming reader
│   │   │   ├── Parser/             # Channel-based async parser
│   │   │   └── Serializer/         # String/stream/file/pretty-print serializer
│   │   ├── Identifiers/            # EdiId, IEdi, ISegmentIdentifier, SegmentFactory
│   │   ├── Separator/              # Separator detection from ISA
│   │   ├── Validation/
│   │   │   ├── Data/               # ValidationMessage, EdiValidationResult, CSV converter
│   │   │   ├── Factory/            # ValidationFactory (Segment + Loop overloads)
│   │   │   ├── SourceGeneration/   # 13 validation attribute classes
│   │   │   └── Validator/          # ValidateEdi recursive walker
│   │   ├── Helper/                 # EnumExtensions, GeneralExtensions, PrettyPrinting/
│   │   ├── Exceptions/             # Custom exception types
│   │   └── Standard/
│   │       ├── Loops/              # FunctionalGroup, ISA/InterchangeEnvelope
│   │       └── Segments/           # ISA, GS, GE, IEA segment definitions
│   │
│   ├── EdiSource.Generator/        # Roslyn source generators (netstandard2.0)
│   │   └── EdiSource.Generator/    # Source root
│   │       ├── LoopGen/            # LoopGenerator and sub-generators
│   │       ├── SegmentGen/         # SegmentGenerator
│   │       ├── ValidationGen/      # ValidationGenerator
│   │       └── Helper/             # CodeWriter, HelperFunctions
│   │
│   └── EdiSource.SampleApp/        # 834 Benefit Enrollment demo app
│       ├── Program.cs              # Full demo: parse, validate, create, modify, serialize
│       ├── Loops/                   # _834, Loop2000, Loop2100
│       └── Segments/               # TS_ST, TS_SE, TS_REF, TS_DTP, Loop2000_*, Loop2100_*
│
├── Implementation/
│   ├── 270/EdiSource.270-5010/     # 270 Eligibility Inquiry (5010)
│   │   ├── _270_5010_EligibilityBenefitInquiry.cs   # Transaction set root
│   │   ├── Loop2000A_InformationSource/              # HL level 20
│   │   ├── Loop2000B_InformationReceiver/            # HL level 21
│   │   ├── Loop2000C_Subscriber/                     # HL level 22
│   │   │   ├── Loop2100C/                            # Subscriber name
│   │   │   │   └── Loop2110C/                        # Eligibility inquiry
│   │   │   └── _270_5010_Loop2000C_Subscriber.cs
│   │   └── Loop2000D_Dependent/                      # HL level 23
│   │       └── Loop2100D/                            # Dependent name
│   │           └── Loop2110D/                        # Dependent inquiry
│   │
│   └── 271/EdiSource.271-5010/     # 271 Eligibility Response (5010)
│       ├── _271_5010_EligibilityBenefitResponse.cs   # Transaction set root
│       ├── Loop1000A/, Loop1000B/                    # Name loops
│       ├── Loop2000A/ through Loop2000D/             # Hierarchical levels
│       └── Loop2120C/, Loop2120D/                    # LS/LE benefit entity grouping
│
└── Tests/
    ├── EdiSource.Domain.Tests/          # Unit tests (Element, Parser, Serializer, Validation)
    ├── EdiSource.Generator.Tests/       # Snapshot tests for generated code
    ├── EdiSource.IntegrationTests/      # End-to-end parse/validate/serialize tests
    ├── EdiSource.270-5010.Tests/        # 270 parsing test
    └── EdiSource.271-5010.Tests/        # 271 parsing test
```

---

## File Location Index

Find things fast:

| Looking for... | Location |
|----------------|----------|
| Public API facade | `Source/EdiSource.Domain/EdiCommon.cs` |
| Element type | `Source/EdiSource.Domain/Elements/Element.cs` |
| Segment base class | `Source/EdiSource.Domain/Segments/Segment.cs` |
| ISegment interface | `Source/EdiSource.Domain/Segments/ISegment.cs` |
| ILoop, ILoopInitialize | `Source/EdiSource.Domain/Loop/` |
| IEdi, IEdi<T> | `Source/EdiSource.Domain/Identifiers/IEdi.cs` |
| EdiId (pattern matching) | `Source/EdiSource.Domain/Identifiers/EdiId.cs` |
| ISegmentIdentifier | `Source/EdiSource.Domain/Identifiers/ISegmentIdentifier.cs` |
| SegmentFactory (near-miss detection) | `Source/EdiSource.Domain/Identifiers/SegmentFactory.cs` |
| EdiReader (streaming) | `Source/EdiSource.Domain/IO/EdiReader/EdiReader.cs` |
| EdiParser (channel-based) | `Source/EdiSource.Domain/IO/Parser/EdiParser.cs` |
| EdiSerializer | `Source/EdiSource.Domain/IO/Serializer/EdiSerializer.cs` |
| Separator detection | `Source/EdiSource.Domain/Separator/Separators.CreateISA.cs` |
| ISA/GS/GE/IEA segments | `Source/EdiSource.Domain/Standard/` |
| InterchangeEnvelope | `Source/EdiSource.Domain/Standard/Loops/ISA/InterchangeEnvelope.cs` |
| FunctionalGroup | `Source/EdiSource.Domain/Standard/Loops/FunctionalGroup.cs` |
| ValidateEdi walker | `Source/EdiSource.Domain/Validation/Validator/ValidateEdi.cs` |
| ValidationMessage | `Source/EdiSource.Domain/Validation/Data/` |
| Validation attributes (13) | `Source/EdiSource.Domain/Validation/SourceGeneration/*.cs` |
| Extension methods | `Source/EdiSource.Domain/Helper/` |
| Exception types | `Source/EdiSource.Domain/Exceptions/` |
| Loop source generator | `Source/EdiSource.Generator/EdiSource.Generator/LoopGen/LoopGenerator.cs` |
| Segment source generator | `Source/EdiSource.Generator/EdiSource.Generator/SegmentGen/SegmentsGenerator.cs` |
| Validation source generator | `Source/EdiSource.Generator/EdiSource.Generator/ValidationGen/ValidationGenerator.cs` |
| CodeWriter (gen helper) | `Source/EdiSource.Generator/EdiSource.Generator/Helper/CodeWriter.cs` |
| 270 transaction set | `Implementation/270/EdiSource.270-5010/` |
| 271 transaction set | `Implementation/271/EdiSource.271-5010/` |
| 834 sample app | `Source/EdiSource.SampleApp/` |
| Domain unit tests | `Tests/EdiSource.Domain.Tests/` |
| Integration tests | `Tests/EdiSource.IntegrationTests/` |
| CI workflow | `.github/workflows/ci.yml` |
| Editor config | `.editorconfig` |

---

## How Parsing Works (End-to-End)

Step-by-step flow when `EdiCommon.ParseEdiEnvelope(ediText)` is called:

```
1. EdiCommon.ParseEdiEnvelope(string)
   └─> Creates MemoryStream + StreamReader from string
   └─> Calls ParseEdiEnvelope(StreamReader)

2. EdiCommon.ParseEdiEnvelope(StreamReader)
   └─> ValidateIsa: checks stream is seekable, length >= 106, starts with "ISA"
   └─> Separators.CreateFromISA: reads positions 3, 104, 105 to detect separators
   └─> Creates EdiParser<InterchangeEnvelope>

3. EdiParser<InterchangeEnvelope>.ParseEdi(StreamReader, Separators)
   └─> Creates Channel.CreateUnbounded<Segment>()
   └─> Launches two concurrent tasks:
       a) EdiReader.ReadEdSegmentsIntoChannelAsync(reader, channel.Writer, separators)
          - Rents 4096-char buffer from ArrayPool
          - Reads chars, splits on separators
          - Writes Segment objects to channel
          - Calls channel.Writer.Complete() in finally
       b) InterchangeEnvelope.InitializeAsync(channel.Reader, null)  [GENERATED]
          - Reads ISA segment from channel
          - Enters while loop, peeks at channel
          - Creates FunctionalGroup children (each reads GS, transaction sets, GE)
          - Transaction sets dispatch to registered definitions
          - Each generated InitializeAsync reads its header, body segments/loops, footer
          - Reads IEA segment
   └─> Task.WhenAll awaits both
   └─> Returns the typed InterchangeEnvelope

4. Result: (InterchangeEnvelope, Separators) tuple
```

**Key insight:** The reader and parser run concurrently. The channel provides backpressure. Segments flow from reader -> channel -> generated InitializeAsync methods, constructing the typed tree as they arrive.

---

## How to Add a New Transaction Set

Follow this pattern (using 835 as an example):

### 1. Create the project

```
Implementation/
  835/
    EdiSource.835-5010/
      EdiSource.835-5010.csproj
      GlobalUsings.cs
      _835_5010_HealthCareClaimPayment.cs
      Segments/
      Loop2000/
      Loop2100/
```

**`.csproj`:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net9.0</TargetFramework>
        <LangVersion>preview</LangVersion>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    <ItemGroup>
        <ProjectReference Include="../../Source/EdiSource.Domain/EdiSource.Domain.csproj" />
        <ProjectReference Include="../../Source/EdiSource.Generator/EdiSource.Generator/EdiSource.Generator.csproj"
                          OutputItemType="Analyzer"
                          ReferenceOutputAssembly="false" />
    </ItemGroup>
</Project>
```

### 2. Define the transaction set root

```csharp
[LoopGenerator<FunctionalGroup, _835_5010, _835_ST>]
public sealed partial class _835_5010 : IValidatable, ITransactionSet<_835_5010>
{
    [SegmentHeader] public _835_ST ST { get; set; } = default!;
    // ... body segments and loops ...
    [SegmentFooter] public _835_SE SE { get; set; } = default!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_835_5010>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate() { /* ... */ }
}
```

### 3. Register it

```csharp
InterchangeEnvelope.TransactionSetDefinitions.Add(_835_5010.Definition);
```

### 4. Add to solution

Add the project to `EdiSource.sln` and create a test project.

---

## How to Add a New Segment

**Template:**

```csharp
[SegmentGenerator<TParentLoop>("TAG", "optional_element1_match")]
public partial class MySegment
{
    // Required property
    public string PropertyName
    {
        get => GetCompositeElement(1);        // 1-based data element index
        set => SetCompositeElement(value, 1);
    }

    // Optional property
    public string? OptionalProp
    {
        get => GetCompositeElementOrNull(2);
        set => value?.Do(x => SetCompositeElement(x, 2));
    }

    // Typed property (int)
    public int Count
    {
        get => this.GetIntRequired(3);
        set => this.SetInt(value, 3);
    }

    // Typed property (DateOnly, format defaults to "yyyyMMdd")
    public DateOnly Date
    {
        get => this.GetDateOnlyRequired(4);
        set => this.SetDateOnly(value, 4);
    }

    // Typed property (decimal)
    public decimal Amount
    {
        get => this.GetDecimalRequired(5);
        set => this.SetDecimal(value, 5);
    }
}
```

**Rules:**
- The class MUST be `partial`
- `TParentLoop` is the loop this segment lives in
- Element index 0 = segment tag (auto-handled)
- Element indices are 1-based for data elements
- Use `GetCompositeElement` for required, `GetCompositeElementOrNull` for optional
- Use extension methods (`GetIntRequired`, `GetDateOnlyRequired`, `GetDateRequired`, `GetBoolRequired`, etc.) for typed access
- Add `IValidatable` if you need custom validation logic
- Add validation attributes for declarative validation

---

## How to Add a New Loop

**Template:**

```csharp
[LoopGenerator<TParent, MySelf, MyHeader>]
public partial class MySelf : IValidatable
{
    // MUST have a header (identifies this loop during parsing)
    [SegmentHeader] public MyHeader Header { get; set; } = default!;

    // Optional single segments
    [Segment] public MyOptionalSegment? Optional { get; set; }

    // Repeating segments
    [SegmentList] public SegmentList<MyRepeatingSegment> Repeats { get; set; } = [];

    // Single child loop
    [Loop] public ChildLoop Child { get; set; } = default!;

    // Repeating child loops
    [LoopList] public LoopList<RepeatingChild> Children { get; set; } = [];

    // Optional footer (e.g., LE segment)
    [OptionalSegmentFooter] public MyFooter? Footer { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        // validation logic
    }
}
```

**Rules:**
- MUST be `partial`
- `TParent` = parent loop, `TSelf` = this type, `THeader` = header segment type
- Properties are parsed in declaration order
- `[SegmentHeader]` is required (exactly one)
- Use `default!` for required non-nullable properties
- Use `[]` (empty collection) for lists
- Use `null`/`T?` for optional single items

---

## How to Add Validation

Three approaches, from simplest to most flexible:

### 1. Validation attributes (declarative, on segments)

```csharp
[SegmentGenerator<MyLoop>("INS")]
[IsOneOfValues(ValidationSeverity.Critical, 1, 0, "Y", "N")]
[ElementLength(ValidationSeverity.Error, 2, 1, 3)]
[BeDate(ValidationSeverity.Warning, 3, 0)]
[RequiredElement(ValidationSeverity.Error, 1, 0)]
public partial class MyINS { /* ... */ }
```

### 2. IValidatable (imperative, on loops or segments)

```csharp
public partial class MyLoop : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        if (Header == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Header required");

        if (Children.Count > 10)
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Max 10 children");

        // Cross-reference validation
        if (Parent is MyParent parent && parent.SomeValue != Header.SomeValue)
            yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Value mismatch");
    }
}
```

### 3. External hooks (from outside the class)

```csharp
IEdi<MyLoop>.Validations.Add(new IndirectValidatable<MyLoop>(loop =>
    loop.Children.Count == 0
        ? [ValidationFactory.Create(loop, ValidationSeverity.Warning, "No children")]
        : null
));
```

---

## Source Generator Patterns

The three generators and what they produce:

### LoopGenerator
- **Input:** `[LoopGenerator<P, S, H>]` on partial class
- **Output files:**
  - `{ClassName}.EdiElement.g.cs` -- `EdiItems` property (ordered list of all child items)
  - `{ClassName}.Implementation.g.cs` -- `ILoop`, `IEdi<P>`, `ISegmentIdentifier<S>` implementations
  - `{ClassName}.TransactionSet.g.cs` -- Only if `ITransactionSet<>` is implemented
  - `{ClassName}.ChannelConstructor.g.cs` -- Empty ctor, ChannelReader ctor, `InitializeAsync` methods

### SegmentGenerator
- **Input:** `[SegmentGenerator<P>("TAG", ...)]` on partial class
- **Output:** `{ClassName}.Implementation.g.cs` -- `IEdi<P>`, `ISegmentIdentifier<S>`, Parent, EdiId, ID-copying ctor

### ValidationGenerator
- **Input:** Validation attributes on classes with `[LoopGeneratorAttribute<>]`
- **Output:** `SourceGenValidations` property returning attribute instances as `IIndirectValidatable[]`

### Important: The Generator project is netstandard2.0

This means:
- No `nullable` annotations
- No file-scoped namespaces
- No records
- No init-only setters
- Uses Polyfill 8.8.1 for some modern APIs

---

## Naming Conventions

| Thing | Pattern | Example |
|-------|---------|---------|
| Transaction set class | `_{code}_{version}_{Name}` | `_270_5010_EligibilityBenefitInquiry` |
| Loop class | `_{code}_{version}_Loop{number}_{Name}` | `_270_5010_Loop2000A_InformationSource` |
| Segment class | `_{code}_{version}_Loop{number}_{Tag}_{Name}` | `_270_5010_Loop2000A_HL_InformationSource` |
| SampleApp segment | `{LoopName}_{Tag}` | `Loop2000_INS`, `TS_ST` |
| SampleApp loop | `Loop{number}` | `Loop2000`, `Loop2100` |
| Test class | `{ClassName}Tests` | `ElementTests`, `EdiParserTests` |
| Segment property | Descriptive name | `EntityIdentifierCode`, `TransactionSetControlNumber` |
| SegmentList property | Plural or descriptive | `REFs`, `DTPs`, `Demographics`, `ContactInfo` |
| LoopList property | `{LoopName}s` or plural | `Loop2100s`, `Loop2000As` |

---

## Common Pitfalls

| Pitfall | Why it happens | How to avoid |
|---------|---------------|--------------|
| `partial` keyword missing | Class won't compile -- generator can't extend it | Always add `partial` to loop/segment classes |
| Wrong parent in `SegmentGenerator<TParent>` | Segment won't be discovered during parsing of the correct loop | `TParent` = the loop this segment belongs to |
| Element index off-by-one | Index 0 is the segment tag, not the first data element | Data elements start at index 1 |
| Property order in loop | Properties parsed in wrong order | Declare properties in X12 segment order |
| Forgot to register transaction set | Parse succeeds but transactions are generic `ILoop`, not your type | Call `InterchangeEnvelope.TransactionSetDefinitions.Add(...)` before parsing |
| Using modern C# in Generator | Generator won't compile | Generator is netstandard2.0 -- use C# 7.3 compatible code |
| Missing `default!` on required loop/segment | NullReferenceException at runtime | Use `= default!` for non-nullable header/body properties |
| Missing `= []` on lists | NullReferenceException when adding items | Initialize `SegmentList<T>` and `LoopList<T>` with `= []` |
| Nullable segment without `?` | Parse fails when segment is absent | Use `T?` type for optional `[Segment]` properties |
| SegmentGenerator multi-value pipe wrong | Matching fails | Use `"2B\|36\|PR"` (pipe-separated in a single string) |

---

## Testing Patterns

### Unit test for a segment

```csharp
[Fact]
public void ShouldParseElement()
{
    var element = new Element(["value1", "value2"]);
    element[0].Should().Be("value1");
    element[1].Should().Be("value2");
}
```

### Integration test for a transaction set

```csharp
[Fact]
public async Task ShouldParse270()
{
    InterchangeEnvelope.TransactionSetDefinitions.Add(
        _270_5010_EligibilityBenefitInquiry.Definition);

    var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);

    var ts = envelope.FunctionalGroups[0].TransactionSets[0]
        as _270_5010_EligibilityBenefitInquiry;

    ts.Should().NotBeNull();
    ts!.ST.Should().NotBeNull();
}
```

### Roundtrip test

```csharp
[Fact]
public async Task ShouldRoundtrip()
{
    var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);
    var serialized = EdiCommon.WriteEdiToString(envelope, separators);
    var (envelope2, _) = await EdiCommon.ParseEdiEnvelope(serialized);
    // Compare structures
}
```

### Test frameworks used

- xUnit 2.9.x
- FluentAssertions
- Verify.Xunit (for snapshot testing)
- NSubstitute (for mocking)

---

## Dependency Graph

```
EdiSource.Generator (netstandard2.0)
    └── Microsoft.CodeAnalysis.CSharp 4.14
    └── Polyfill 8.8.1

EdiSource.Domain (net9.0)
    └── (no external dependencies)

EdiSource.270-5010 (net9.0)
    ├── EdiSource.Domain
    └── EdiSource.Generator (as Analyzer)

EdiSource.271-5010 (net9.0)
    ├── EdiSource.Domain
    └── EdiSource.Generator (as Analyzer)

EdiSource.SampleApp (net9.0)
    ├── EdiSource.Domain
    └── EdiSource.Generator (as Analyzer)

EdiSource.Domain.Tests (net9.0)
    ├── EdiSource.Domain
    ├── xUnit, FluentAssertions, Verify.Xunit, NSubstitute

EdiSource.IntegrationTests (net9.0)
    ├── EdiSource.Domain
    ├── EdiSource.Generator (as Analyzer)
    ├── xUnit, FluentAssertions, Verify.Xunit, NSubstitute
```

---

## Key Interfaces and What They Do

| Interface | Purpose | Who implements |
|-----------|---------|----------------|
| `IEdi` | Marker for all EDI types | All segments, loops, segment lists, loop lists |
| `IEdi<T>` | Typed parent reference + static validation hooks | Generated on all segments/loops |
| `ILoop` | Marks a loop; provides `EdiItems` | Generated on all loops |
| `ILoopInitialize<T>` | Static `InitializeAsync` factory methods | Generated on all loops |
| `ISegment` | Element access API | `Segment` base class |
| `ISegmentIdentifier<T>` | `EdiId` + `MatchesAsync` for segment identity | Generated on all segments and loops |
| `ITransactionSet<T>` | Marks a loop as a transaction set root | User-defined transaction set classes |
| `IValidatable` | `Validate()` method returning `IEnumerable<ValidationMessage>` | User-defined loops/segments |
| `ISourceGeneratorValidatable` | `SourceGenValidations` property | Generated when validation attributes present |
| `IIndirectValidatable` | `Validate(IEdi)` for attribute-driven validation | Validation attribute classes |
| `IUserValidation<T>` | Static `UserValidations` list for external hooks | Interface with default implementation |

---

## Element Indexing Rules

```
Segment: NM1*IL*1*DOE*JOHN*A***34*123456789
          |   |  |  |   |   | | |  |    |
Index:    0   1  2  3   4   5 6 7  8    9

Element 0 = "NM1" (segment tag)
Element 1 = "IL" (first data element)
Element 2 = "1"
Element 3 = "DOE"
...

For composite elements:  HI*ABK:S7200
Element 1, Composite 0 = "ABK"
Element 1, Composite 1 = "S7200"
```

**Access patterns:**
```csharp
segment.GetCompositeElement(1)        // Element 1, Composite 0 (throws if missing)
segment.GetCompositeElementOrNull(1)  // Element 1, Composite 0 (null if missing)
segment.GetCompositeElement(1, 1)     // Element 1, Composite 1 (throws)
segment[1]                            // Shorthand for Element 1, Composite 0
segment[1, 1]                         // Shorthand for Element 1, Composite 1
```

---

## EdiId Pattern Matching Rules

`EdiId` is constructed from segment tag + optional element values:

```csharp
// Matches: any segment with tag "NM1"
new EdiId("NM1")

// Matches: NM1 where element[1] == "IL"
new EdiId("NM1", "IL")

// Matches: NM1 where element[1] is "2B" OR "36" OR "PR"
new EdiId("NM1", "2B|36|GP|P5|PR")

// Matches: HL where element[3] == "20" (null = wildcard for elements 1, 2)
new EdiId("HL", null, null, "20")
```

In `[SegmentGenerator]` attributes:
```csharp
[SegmentGenerator<MyLoop>("NM1", "2B|36|PR")]  // Tag + multi-value element 1
[SegmentGenerator<MyLoop>("HL", null, null, "20")]  // Tag + element 3 match
```

---

## Validation System Layers

Validation runs in this order for each EDI item during tree walking:

```
For Loops:
  1. IUserValidation<T>.UserValidations  (external hooks)
  2. IValidatable.Validate()              (manual validation)
  3. ISourceGeneratorValidatable          (attribute-based validation)
  4. Recurse into EdiItems children

For Segments:
  1. IValidatable.Validate()              (manual validation)
  2. ISourceGeneratorValidatable          (attribute-based validation)
  3. IUserValidation<T>.UserValidations  (external hooks)
```

Each `ValidationMessage` gets `LoopLine` and `SegmentLine` annotations for location tracking.

---

## Envelope Structure

```
InterchangeEnvelope
├── ISA (fixed 16-element segment)
├── FunctionalGroups[] (LoopList)
│   ├── GS
│   ├── TransactionSets[] (LoopList<ILoop>)
│   │   └── (Your registered transaction set type)
│   │       ├── ST (header)
│   │       ├── ... (body loops and segments)
│   │       └── SE (footer)
│   └── GE
└── IEA
```

The `TransactionSets` list contains `ILoop` instances. They are cast to your specific type after parsing:

```csharp
var ts = envelope.FunctionalGroups[0].TransactionSets[0] as _834;
```

---

## Areas for Improvement

### High-Impact, Low-Effort
- **Auto segment count in SE/GE/IEA**: These trailer segments require manual count values. Auto-calculating during serialization would eliminate a common user error.
- **Validation message enrichment**: Add segment tag and loop type name to `ValidationMessage` (currently only has line numbers).
- **Roslyn diagnostics in generators**: Emit warnings/errors when attribute parameters are invalid (e.g., wrong element index, invalid severity) instead of silently generating broken code.

### Medium-Impact
- **Streaming error recovery**: Allow "best-effort" parsing that skips unrecognized segments instead of throwing. Collect warnings about skipped segments.
- **Declarative max-repeat counts**: A `[MaxCount(9)]` attribute on `SegmentList`/`LoopList` properties for automatic count validation.
- **Partial ISA/GS parsing**: Parse just the envelope headers for routing without parsing the full transaction body.
- **Cross-segment validation framework**: Built-in rules for SE count matching, HL hierarchy validation, GE/IEA count matching.

### Architectural
- **EdiReader buffer handling**: `ReadAsync` return value is unused -- the loop always processes `BufferSize` characters, which may process garbage/zeros from the previous read when the file doesn't fill the buffer exactly. This should use the actual chars-read count.
- **EDIFACT support**: Requires abstracting the separator scheme and segment structure. Currently tightly coupled to X12.
- **Read-only parsed models**: No immutable parsed representation. All parsed objects are mutable.
- **Incremental generator caching**: The generators could cache more aggressively to avoid re-running on non-EDI file changes.

### Testing
- **Edge case coverage**: Missing optional segments, max-count boundaries, malformed ISA, empty transactions, multi-GS envelopes.
- **Benchmarks**: No BenchmarkDotNet suite exists. The streaming pipeline's throughput characteristics are unmeasured.
- **Property-based testing**: Random valid EDI generation -> parse -> serialize -> compare would catch serialization fidelity bugs.
- **Generator error tests**: Verify that invalid attribute combinations produce helpful errors (currently no diagnostics are emitted).

### Developer Experience
- **NuGet publication**: Package metadata is configured but no package is published.
- **IDE integration**: A Roslyn analyzer that suggests validation attributes based on element usage patterns.
- **XML docs on generated code**: Generated properties and methods lack XML documentation, reducing IntelliSense usefulness.
- **Migration guide**: No documentation for upgrading between versions or migrating from other EDI libraries.
