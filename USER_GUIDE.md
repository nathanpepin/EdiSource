# EdiSource User Guide

A practical guide for using EdiSource to parse, validate, create, modify, and serialize X12 EDI documents in .NET 9.

---

## Table of Contents

- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Requirements](#requirements)
  - [Project Setup](#project-setup)
- [Parsing EDI Documents](#parsing-edi-documents)
  - [Parse a Full Envelope](#parse-a-full-envelope)
  - [Parse from Different Sources](#parse-from-different-sources)
  - [Register Transaction Sets](#register-transaction-sets)
  - [Access Parsed Data](#access-parsed-data)
  - [Parse a Standalone Transaction](#parse-a-standalone-transaction)
  - [Parse Into Raw Segments](#parse-into-raw-segments)
- [Creating EDI Documents Programmatically](#creating-edi-documents-programmatically)
  - [Build an Envelope from Scratch](#build-an-envelope-from-scratch)
  - [Create the ISA Segment](#create-the-isa-segment)
- [Modifying EDI Documents](#modifying-edi-documents)
  - [Change Segment Values](#change-segment-values)
  - [Add Segments and Loops](#add-segments-and-loops)
- [Serialization](#serialization)
  - [Write to String](#write-to-string)
  - [Write to Stream](#write-to-stream)
  - [Write to File](#write-to-file)
  - [Pretty Print](#pretty-print)
- [Validation](#validation)
  - [Validate a Document](#validate-a-document)
  - [Filter by Severity](#filter-by-severity)
  - [Export to CSV](#export-to-csv)
- [Building Custom Transaction Sets](#building-custom-transaction-sets)
  - [Step 1: Create the Project](#step-1-create-the-project)
  - [Step 2: Define Segments](#step-2-define-segments)
  - [Step 3: Define Loops](#step-3-define-loops)
  - [Step 4: Define the Transaction Set](#step-4-define-the-transaction-set)
  - [Step 5: Register and Parse](#step-5-register-and-parse)
- [Segment Definition Reference](#segment-definition-reference)
  - [Basic Segment Properties](#basic-segment-properties)
  - [Typed Accessors](#typed-accessors)
  - [Segment Identity Matching](#segment-identity-matching)
  - [Nullable vs Required Properties](#nullable-vs-required-properties)
  - [Parent Access](#parent-access)
- [Loop Definition Reference](#loop-definition-reference)
  - [Loop Property Attributes](#loop-property-attributes)
  - [LoopGenerator Attribute](#loopgenerator-attribute)
- [Validation Attributes](#validation-attributes)
  - [Attribute-Based Validation](#attribute-based-validation)
  - [Manual Validation (IValidatable)](#manual-validation-ivalidatable)
  - [External Validation Hooks](#external-validation-hooks)
  - [Available Validation Attributes](#available-validation-attributes)
- [Working With Elements Directly](#working-with-elements-directly)
  - [Element Implicit Conversions](#element-implicit-conversions)
  - [Composite Elements](#composite-elements)
- [Using Included Transaction Sets](#using-included-transaction-sets)
  - [270 Eligibility Inquiry](#270-eligibility-inquiry)
  - [271 Eligibility Response](#271-eligibility-response)
- [Custom Separators](#custom-separators)
- [Error Handling](#error-handling)
- [Areas for Improvement](#areas-for-improvement)

---

## Getting Started

### Installation

Reference the `EdiSource.Domain` NuGet package (or project) and the `EdiSource.Generator` as an analyzer:

```xml
<ItemGroup>
    <ProjectReference Include="path/to/EdiSource.Domain.csproj" />
    <ProjectReference Include="path/to/EdiSource.Generator.csproj"
                      OutputItemType="Analyzer"
                      ReferenceOutputAssembly="false" />
</ItemGroup>
```

For pre-built transaction sets, also reference the implementation projects:

```xml
<ProjectReference Include="path/to/EdiSource.270-5010.csproj" />
<ProjectReference Include="path/to/EdiSource.271-5010.csproj" />
```

### Requirements

- .NET 9.0 SDK
- C# 13 (`<LangVersion>preview</LangVersion>`)
- `<Nullable>enable</Nullable>` recommended

### Project Setup

Add these to your `.csproj`:

```xml
<PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <LangVersion>preview</LangVersion>
    <Nullable>enable</Nullable>
</PropertyGroup>
```

Add a `GlobalUsings.cs` with the common imports:

```csharp
global using EdiSource.Domain;
global using EdiSource.Domain.Elements;
global using EdiSource.Domain.Segments;
global using EdiSource.Domain.Segments.Extensions;
global using EdiSource.Domain.Loop;
global using EdiSource.Domain.Identifiers;
global using EdiSource.Domain.SourceGeneration;
global using EdiSource.Domain.Validation.Data;
global using EdiSource.Domain.Validation.Factory;
global using EdiSource.Domain.Validation.SourceGeneration;
global using EdiSource.Domain.Standard.Loops;
global using static EdiSource.Domain.Identifiers.EdiId;
```

---

## Parsing EDI Documents

### Parse a Full Envelope

The most common entry point. This parses the complete ISA/GS/ST hierarchy:

```csharp
using EdiSource.Domain;

// Register any custom transaction sets first
InterchangeEnvelope.TransactionSetDefinitions.Add(MyTransactionSet.Definition);

// Parse
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);

// Access the structure
var isa = envelope.ISA;
var functionalGroup = envelope.FunctionalGroups[0];
var transaction = functionalGroup.TransactionSets[0] as MyTransactionSet;
```

### Parse from Different Sources

```csharp
// From a string
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);

// From a file
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(new FileInfo("input.edi"));

// From a StreamReader
using var stream = new StreamReader(fileStream);
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(stream);
```

All parse methods support `CancellationToken`:

```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText, cts.Token);
```

### Register Transaction Sets

Before parsing, register your transaction set definitions so the parser knows how to construct typed transactions from ST segments:

```csharp
// Register the 270 (Eligibility Inquiry) implementation
InterchangeEnvelope.TransactionSetDefinitions.Add(
    _270_5010_EligibilityBenefitInquiry.Definition
);

// Register multiple
InterchangeEnvelope.TransactionSetDefinitions.Add(_270_5010_EligibilityBenefitInquiry.Definition);
InterchangeEnvelope.TransactionSetDefinitions.Add(_271_5010_EligibilityBenefitResponse.Definition);
```

If a transaction set is not registered, the parser will still read the segments but won't construct a typed object -- they'll remain as generic `ILoop` instances.

### Access Parsed Data

Once parsed, navigate the typed object graph:

```csharp
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);

// Envelope level
Console.WriteLine($"Sender: {envelope.ISA.InterchangeSenderId}");
Console.WriteLine($"Receiver: {envelope.ISA.InterchangeReceiverId}");

// Functional group level
var fg = envelope.FunctionalGroups[0];
Console.WriteLine($"Group Control: {fg.GS.E06GroupControlNumber}");

// Transaction level (cast to your registered type)
var transaction = fg.TransactionSets[0] as _834;
Console.WriteLine($"Control #: {transaction.ST.TransactionSetControlNumber}");

// Loop data
var insured = transaction.Loop2000;
Console.WriteLine($"Insured: {insured.INS.InsuredIndicator}");

// Repeating loops
foreach (var member in transaction.Loop2100s)
{
    Console.WriteLine($"Name: {member.NM1.FirstName} {member.NM1.LastName}");
}
```

### Parse a Standalone Transaction

If you have just a transaction (without ISA/GS envelope), parse directly into the loop type:

```csharp
var transaction = await EdiCommon.ParseEdi<_834>(transactionText, separators);
```

### Parse Into Raw Segments

For inspection or custom processing, parse to a flat segment list:

```csharp
List<Segment> segments = await EdiCommon.ParseIntoSegments(ediText, separators);

foreach (var segment in segments)
{
    var tag = segment.GetCompositeElement(0, 0); // "ISA", "GS", "ST", etc.
    Console.WriteLine($"{tag}: {segment}");
}
```

---

## Creating EDI Documents Programmatically

### Build an Envelope from Scratch

Use object initializer syntax to build the full hierarchy:

```csharp
var envelope = new InterchangeEnvelope
{
    ISA = ISA.CreateDefault("ZZ", "SENDER         ", "ZZ", "RECEIVER       ", 12345),
    FunctionalGroups =
    [
        new FunctionalGroup
        {
            GS = new GS { E06GroupControlNumber = "1" },
            TransactionSets =
            [
                new _834
                {
                    ST = new TS_ST { TransactionSetControlNumber = "0001" },
                    REFs = [ new TS_REF { ReferenceQualifier = "38", ReferenceId = "POLICY123" } ],
                    DTP = new TS_DTP
                    {
                        DateQualifier = "356",
                        DateFormatQualifier = "D8",
                        Date = DateTime.Now
                    },
                    Loop2000 = new Loop2000
                    {
                        INS = new Loop2000_INS
                        {
                            InsuredIndicator = "Y",
                            IndividualRelationshipCode = "18"
                        }
                    },
                    Loop2100s =
                    [
                        new Loop2100
                        {
                            NM1 = new Loop2100_NM1
                            {
                                EntityIdentifierCode = "IL",
                                EntityTypeQualifier = "1",
                                LastName = "DOE",
                                FirstName = "JOHN"
                            }
                        }
                    ],
                    SE = new TS_SE
                    {
                        SegmentCount = 8,
                        TransactionSetControlNumber = "0001"
                    }
                }
            ],
            GE = new GE { E01NumberOfTransactionSets = 1, E02GroupControlNumber = "1" }
        }
    ],
    IEA = new IEA { E01NumberOfFunctionalGroups = 1, E02InterchangeControlNumber = "000012345" }
};
```

### Create the ISA Segment

`ISA.CreateDefault` provides a convenience factory:

```csharp
var isa = ISA.CreateDefault(
    senderQualifier: "ZZ",
    senderId: "SENDER         ",    // 15 characters, padded
    receiverQualifier: "ZZ",
    receiverId: "RECEIVER       ",   // 15 characters, padded
    controlNumber: 12345
);
```

---

## Modifying EDI Documents

Parsed EDI documents are fully mutable. You can change any property, add segments, or add loops.

### Change Segment Values

```csharp
// Change a name
var member = transaction.Loop2100s[0];
member.NM1.FirstName = "JONATHAN";
member.NM1.LastName = "SMITH";

// Change a date
transaction.DTP.Date = DateTime.Now;
```

### Add Segments and Loops

```csharp
// Add a reference segment to a list
transaction.REFs.Add(new TS_REF
{
    ReferenceQualifier = "ZZ",
    ReferenceId = "NEW-REF-VALUE"
});

// Add a new member loop
transaction.Loop2100s.Add(new Loop2100
{
    NM1 = new Loop2100_NM1
    {
        EntityIdentifierCode = "IL",
        EntityTypeQualifier = "1",
        LastName = "NEWMEMBER",
        FirstName = "ALICE"
    },
    Demographics =
    [
        new Loop2100_DMG { DateOfBirth = new DateTime(1995, 3, 20), Gender = "F" }
    ]
});
```

---

## Serialization

### Write to String

```csharp
string ediOutput = EdiCommon.WriteEdiToString(envelope, separators, includeNewLine: true);
```

`includeNewLine` adds a newline after each segment separator for readability. Set to `false` for compact output.

### Write to Stream

```csharp
await EdiCommon.WriteEdi(envelope, outputStream, separators, includeNewLine: true, leaveOpen: true);
```

### Write to File

```csharp
await EdiCommon.WriteEdi(envelope, new FileInfo("output.edi"), separators);
```

### Pretty Print

Generates a human-readable hierarchical representation:

```csharp
string prettyOutput = EdiCommon.PrettyPrint(envelope);
Console.WriteLine(prettyOutput);
```

Output looks like:

```
ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *...
  GS*BE*SENDER*RECEIVER*...
    ST*834*0001
    REF*38*123456789
    DTP*356*D8*20230701
    INS*Y*18*030*XN*A*E**FT
      REF*0F*12345
      ...
    SE*16*0001
  GE*1*1
IEA*1*000000001
```

---

## Validation

### Validate a Document

```csharp
var result = EdiCommon.Validate(envelope);

if (result.IsValid)
{
    Console.WriteLine("Document is valid.");
}
else
{
    foreach (var msg in result.ValidationMessages)
    {
        Console.WriteLine($"[{msg.Severity}] Line {msg.SegmentLine}: {msg.Message}");
    }
}
```

### Filter by Severity

```csharp
var criticals = result.ValidationMessages
    .Where(m => m.Severity == ValidationSeverity.Critical);

var errorsAndAbove = result.ValidationMessages
    .Where(m => m.Severity >= ValidationSeverity.Error);
```

Severity levels (from most to least severe): `Critical` (4), `Error` (3), `Warning` (2), `Info` (1). Higher numeric value = more severe.

`EdiValidationResult` also provides convenience properties: `.WhereHasCritical`, `.WhereHasError`, `.WhereHasWarning`.

### Export to CSV

```csharp
await EdiCommon.WriteValidationsToCsvFile(result, new FileInfo("validation_results.csv"));
```

---

## Building Custom Transaction Sets

This is the core workflow for implementing a new X12 transaction type.

### Step 1: Create the Project

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net9.0</TargetFramework>
        <LangVersion>preview</LangVersion>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    <ItemGroup>
        <ProjectReference Include="../EdiSource.Domain/EdiSource.Domain.csproj" />
        <ProjectReference Include="../EdiSource.Generator/EdiSource.Generator/EdiSource.Generator.csproj"
                          OutputItemType="Analyzer"
                          ReferenceOutputAssembly="false" />
    </ItemGroup>
</Project>
```

### Step 2: Define Segments

Each segment is a `partial class` decorated with `[SegmentGenerator<TParentLoop>]`:

```csharp
// ST segment that identifies this as an 834 transaction
[SegmentGenerator<_834>("ST", "834")]
public partial class TS_ST : IValidatable
{
    // Element 2 = Transaction Set Control Number
    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (string.IsNullOrWhiteSpace(TransactionSetControlNumber))
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Transaction Set Control Number is required");
    }
}
```

**`SegmentGenerator` parameters:**
1. Generic type parameter: The parent loop this segment belongs to
2. First string: Segment tag (e.g., "ST", "NM1", "HL")
3. Additional strings (optional): Element values for identity matching

```csharp
// Matches any NM1 segment
[SegmentGenerator<MyLoop>("NM1")]

// Matches NM1 where element 1 is "IL"
[SegmentGenerator<MyLoop>("NM1", "IL")]

// Matches NM1 where element 1 is any of: 2B, 36, GP, P5, PR
[SegmentGenerator<MyLoop>("NM1", "2B|36|GP|P5|PR")]

// Matches HL where element 3 is "20" (skip elements with null)
[SegmentGenerator<MyLoop>("HL", null, null, "20")]
```

### Step 3: Define Loops

Loops are hierarchical containers that hold segments and child loops:

```csharp
[LoopGenerator<_834, Loop2000, Loop2000_INS>]
public partial class Loop2000 : IValidatable
{
    // Header segment (required, identifies this loop)
    [SegmentHeader] public Loop2000_INS INS { get; set; } = default!;

    // Optional repeating segments
    [SegmentList] public SegmentList<Loop2000_REF> REFs { get; set; } = [];
    [SegmentList] public SegmentList<Loop2000_DTP> DTPs { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (INS.IndividualRelationshipCode != "18")
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning,
                $"Unusual relationship code: {INS.IndividualRelationshipCode}");
    }
}
```

**`LoopGenerator<TParent, TSelf, THeader>` type parameters:**
1. `TParent` -- The parent loop (or `FunctionalGroup` for transaction sets)
2. `TSelf` -- This loop's own type (for self-referencing in generated code)
3. `THeader` -- The header segment type that identifies this loop

### Step 4: Define the Transaction Set

The root loop implements `ITransactionSet<T>` and provides a static `Definition`:

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

    // This is how the parser discovers this transaction set type
    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_834>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        if (Loop2000 == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Insured loop (2000) is required");
    }
}
```

### Step 5: Register and Parse

```csharp
// Register once at application startup
InterchangeEnvelope.TransactionSetDefinitions.Add(_834.Definition);

// Parse
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);
var transaction = envelope.FunctionalGroups[0].TransactionSets[0] as _834;

// Use the typed data
Console.WriteLine(transaction.ST.TransactionSetControlNumber);
Console.WriteLine(transaction.Loop2000.INS.InsuredIndicator);
```

---

## Segment Definition Reference

### Basic Segment Properties

Element access uses 0-based indexing where element 0 is the segment tag:

```csharp
[SegmentGenerator<MyLoop>("NM1")]
public partial class MyNM1
{
    // Element 0 = "NM1" (the tag -- auto-handled)
    // Element 1 = Entity Identifier Code
    public string EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    // Element 3 = Last Name
    public string LastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    // Optional element -- use OrNull variant
    public string? MiddleName
    {
        get => GetCompositeElementOrNull(5);
        set => value?.Do(x => SetCompositeElement(x, 5));
    }
}
```

### Typed Accessors

Extension methods on `Segment` provide type-safe parsing. All methods take a `dataElement` index (1-based) and an optional `compositeElement` index (defaults to 0):

```csharp
// Integer
public int SegmentCount
{
    get => this.GetIntRequired(1);
    set => this.SetInt(value, 1);
}

// Decimal
public decimal MonetaryAmount
{
    get => this.GetDecimalRequired(2);
    set => this.SetDecimal(value, 2);
}

// DateOnly (format defaults to "yyyyMMdd")
public DateOnly DateOfBirth
{
    get => this.GetDateOnlyRequired(2);
    set => this.SetDateOnly(value, 2);
}

// DateTime (format defaults to "yyyyMMdd")
public DateTime Date
{
    get => this.GetDateRequired(3);
    set => this.SetDate(value, 3);
}

// TimeOnly (format defaults to "HHmm")
public TimeOnly ServiceTime
{
    get => this.GetTimeOnlyRequired(5);
    set => this.SetTimeOnly(value, 5);
}

// Boolean (requires trueValue; falseValue defaults to "N")
public bool HierarchicalChildCode
{
    get => this.GetBoolRequired(4, "1");
    set => this.SetBool(value, "1", "0", 4);
}
```

Nullable variants (`GetInt`, `GetDecimal`, `GetDateOnly`, `GetDate`, `GetTimeOnly`, `GetBool`) return `T?` and return `null` when the element is absent.

### Segment Identity Matching

The `SegmentGenerator` attribute parameters control which incoming segments match this type:

| Pattern | Matches |
|---------|---------|
| `("ST")` | Any segment with tag "ST" |
| `("ST", "834")` | ST segment where element 1 = "834" |
| `("NM1", "2B\|36\|PR")` | NM1 where element 1 is "2B", "36", or "PR" |
| `("HL", null, null, "20")` | HL where element 3 = "20" (elements 1-2 are wildcards) |

### Nullable vs Required Properties

```csharp
// Required -- throws if element doesn't exist
public string EntityCode
{
    get => GetCompositeElement(1);    // throws ArgumentOutOfRangeException if missing
    set => SetCompositeElement(value, 1);
}

// Optional -- returns null if element doesn't exist
public string? MiddleName
{
    get => GetCompositeElementOrNull(5);  // returns null if missing
    set => value?.Do(x => SetCompositeElement(x, 5));
}
```

### Parent Access

Every segment and loop has a `Parent` property pointing to its containing loop. This enables cross-segment validation:

```csharp
[SegmentGenerator<_834>("SE")]
public partial class TS_SE : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        // Access the parent transaction to cross-reference ST
        if (Parent is _834 transaction &&
            TransactionSetControlNumber != transaction.ST.TransactionSetControlNumber)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "SE control number must match ST");
    }
}
```

---

## Loop Definition Reference

### Loop Property Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `[SegmentHeader]` | Single segment | **Required.** The first segment that identifies this loop. Consumed first during parsing. |
| `[Segment]` | Single segment (nullable) | Optional segment. Use `T?` type. Consumed if the next segment matches. |
| `[SegmentList]` | `SegmentList<T>` | Zero or more repeating segments. All consecutive matches are consumed. |
| `[Loop]` | Single loop | Required child loop. Recursively initialized. |
| `[LoopList]` | `LoopList<T>` | Zero or more child loops. Each match triggers recursive initialization. |
| `[SegmentFooter]` | Single segment | Required closing segment (e.g., SE, GE, IEA). Consumed last. |
| `[OptionalSegmentFooter]` | Single segment (nullable) | Optional closing segment (e.g., LE). Skipped if not present. |

**Property declaration order matters.** The source generator processes properties in the order they appear in the class. Segments and loops are matched in this order during parsing.

### LoopGenerator Attribute

```csharp
[LoopGenerator<TParent, TSelf, THeader>]
```

| Parameter | Description |
|-----------|-------------|
| `TParent` | The parent loop type. Use `FunctionalGroup` for transaction set roots. |
| `TSelf` | The loop's own type. Required for self-referencing in generated code. |
| `THeader` | The header segment type. Its `EdiId` is used to identify this loop. |

---

## Validation Attributes

### Attribute-Based Validation

Place validation attributes on segment classes. The source generator emits code to run them automatically:

```csharp
[SegmentGenerator<Loop2000>("INS")]
[IsOneOfValues(ValidationSeverity.Critical, 1, 0, "Y", "N")]  // Element 1 must be "Y" or "N"
public partial class Loop2000_INS
{
    public string InsuredIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}
```

```csharp
[SegmentGenerator<Loop2000>("DTP")]
[BeDate(ValidationSeverity.Critical, 3, 0)]  // Element 3 must be a valid date
public partial class Loop2000_DTP
{
    public DateTime Date
    {
        get => this.GetDateRequired(3);
        set => this.SetDate(value, 3);
    }
}
```

```csharp
[SegmentGenerator<_834>("REF")]
[ElementLength(ValidationSeverity.Error, 1, 1, 3)]  // Element 1 length: 1-3 chars
public partial class TS_REF
{
    public string ReferenceQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}
```

### Manual Validation (IValidatable)

Implement `IValidatable` on any loop or segment for custom logic:

```csharp
public partial class Loop2100 : IValidatable
{
    public IEnumerable<ValidationMessage> Validate()
    {
        if (string.IsNullOrWhiteSpace(NM1.LastName))
            yield return ValidationFactory.Create(this, ValidationSeverity.Error,
                "Member last name is required");

        if (Demographics.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning,
                "Demographic information is recommended");
    }
}
```

### External Validation Hooks

Add validation rules from outside the class using `IEdi<T>.Validations`:

```csharp
// Add a validation rule to GS segments from application code
IEdi<FunctionalGroup>.Validations.Add(new IndirectValidatable<FunctionalGroup>(fg =>
    fg.GS.GetCompositeElementOrNull(0) is null
        ? [ValidationFactory.CreateCritical(fg, "GS functional identifier is missing")]
        : null
));
```

Or via static constructors in segment classes:

```csharp
static GS()
{
    ValidationHelper.Add<GS>(x => x.GetCompositeElementOrNull(0) is null
        ? [ValidationFactory.CreateCritical(x, "This makes no sense")]
        : null);
}
```

### Available Validation Attributes

| Attribute | Parameters | Description |
|-----------|------------|-------------|
| `[ElementLength]` | severity, dataElement, min, max | Total element length must be between min and max |
| `[ElementLength]` | severity, dataElement, length | Total element length must be exactly `length` |
| `[CompositeElementLength]` | severity, dataElement, compositeElement, min, max | Single composite element length range |
| `[Empty]` | severity, dataElement, compositeElement | Element must be empty or absent |
| `[NotEmpty]` | severity, dataElement, compositeElement | Element must be present and non-empty |
| `[IsOneOfValues]` | severity, dataElement, compositeElement, params values | Must be one of the listed values |
| `[NotOneOfValues]` | severity, dataElement, compositeElement, params values | Must NOT be one of the listed values |
| `[RequiredDataElements]` | severity, params elementIndices | Listed data elements must exist |
| `[RequiredElement]` | severity, dataElement, compositeElement | Specific composite element must exist and be non-empty |
| `[BeDate]` | severity, dataElement, compositeElement | Must parse as a valid date |
| `[BeDateTime]` | severity, dataElement, compositeElement | Must parse as a valid datetime |
| `[BeTime]` | severity, dataElement, compositeElement | Must parse as a valid time |
| `[BeInt]` | severity, dataElement, compositeElement | Must parse as an integer |
| `[BeDecimal]` | severity, dataElement, compositeElement | Must parse as a decimal |

**Severity levels:** `Critical` > `Error` > `Warning` > `Info`

Multiple attributes can be stacked on a single segment class. They are all evaluated during validation.

---

## Working With Elements Directly

### Element Implicit Conversions

`Element` supports implicit conversions for convenience:

```csharp
Element element = "hello";          // string -> Element
Element multi = ["a", "b", "c"];    // string[] -> Element (composite)

string value = element;             // Element -> string (first composite)
string[] values = multi;            // Element -> string[]
```

### Composite Elements

Some X12 elements contain sub-components separated by `:` (the composite separator). Access them with the two-index overload:

```csharp
// HI segment: HealthCareCodeInformation is a composite element
// HI*ABK:S7200*ABF:78901
//   Element 1, Composite 0 = "ABK"
//   Element 1, Composite 1 = "S7200"

string codeQualifier = segment.GetCompositeElement(1, 0);  // "ABK"
string code = segment.GetCompositeElement(1, 1);            // "S7200"
```

---

## Using Included Transaction Sets

### 270 Eligibility Inquiry

```csharp
using EdiSource._270_5010;

// Register
InterchangeEnvelope.TransactionSetDefinitions.Add(
    _270_5010_EligibilityBenefitInquiry.Definition
);

// Parse
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);
var ts = envelope.FunctionalGroups[0].TransactionSets[0]
    as _270_5010_EligibilityBenefitInquiry;

// Access subscriber info
var subscriber = ts.Loop2000Cs[0];
var subscriberName = subscriber.Loop2100C.NM1;
Console.WriteLine($"Subscriber: {subscriberName.SubscriberFirstName} {subscriberName.SubscriberLastName}");

// Access eligibility inquiry
var inquiry = subscriber.Loop2110Cs[0];
Console.WriteLine($"Service Type: {inquiry.EQ.ServiceTypeCode}");
```

### 271 Eligibility Response

```csharp
using EdiSource._271_5010;

InterchangeEnvelope.TransactionSetDefinitions.Add(
    _271_5010_EligibilityBenefitResponse.Definition
);

var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);
var ts = envelope.FunctionalGroups[0].TransactionSets[0]
    as _271_5010_EligibilityBenefitResponse;

// Access eligibility/benefit information
var subscriber = ts.Loop2000As[0].Loop2000Bs[0].Loop2000Cs[0];
var benefitInfo = subscriber.Loop2100Cs[0].Loop2110Cs[0];
Console.WriteLine($"EB: {benefitInfo.EB.EligibilityOrBenefitInformation}");
```

---

## Custom Separators

By default, separators are auto-detected from the ISA segment. You can also specify them explicitly:

```csharp
// Default X12 separators
var separators = Separators.DefaultSeparators;
// SegmentSeparator = '~'
// DataElementSeparator = '*'
// CompositeElementSeparator = ':'

// Custom separators (constructor order: segment, dataElement, composite)
var custom = new Separators('~', '|', ':');

// Parse with custom separators
var result = await EdiCommon.ParseEdi<MyTransaction>(ediText, custom);
```

---

## Error Handling

EdiSource provides descriptive exceptions with diagnostic context:

```csharp
try
{
    var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediText);
}
catch (InvalidISAException ex)
{
    // ISA segment is missing, too short, or stream is not seekable
    Console.WriteLine($"Invalid ISA: {ex.Message}");
}
catch (EdiParsingException ex)
{
    // Segment mismatch during parsing -- includes near-miss diagnostics
    Console.WriteLine(ex.Message);
    // The message includes:
    // - Expected vs received segment
    // - Parsing context (current loop)
    // - Close matches with suggested fixes
    // - Common troubleshooting steps
}
catch (EdiReaderException ex)
{
    // No segments could be read
    Console.WriteLine($"Reader error: {ex.Message}");
}
catch (DataElementParsingError ex)
{
    // Element type conversion failed (e.g., "ABC" as integer)
    Console.WriteLine($"Data error: {ex.Message}");
}
```

---

## Areas for Improvement

### For Users Building Implementations
- **Segment count calculation**: The SE segment's segment count must be manually calculated. An auto-counting utility would eliminate this error-prone step.
- **HL hierarchy validation**: HL parent/child ID relationships require manual validation code. A built-in HL hierarchy walker would simplify 270/271 and similar hierarchical transactions.
- **Segment max-repeat limits**: There's no declarative way to say "max 9 REF segments in this loop." A `[MaxCount(9)]` attribute on `SegmentList` properties would enable automatic validation.
- **Element length auto-validation**: Element min/max lengths from X12 specs must be manually added as `[ElementLength]` attributes. Auto-generation from spec tables would reduce tedium.

### For the Parsing Experience
- **Streaming error recovery**: A single parsing error (unexpected segment) aborts the entire parse. A "best-effort" mode that skips unrecognized segments and logs warnings would help with non-compliant EDI files.
- **Partial parsing**: No way to parse just the ISA/GS headers without parsing the full transaction body. Useful for routing/filtering large files.
- **Progress reporting**: For large files, the async pipeline has no progress callback. An `IProgress<int>` parameter on parse methods would help.

### For Validation
- **Auto cross-reference validation**: SE count, GE count, IEA count, and HL hierarchy validation are all standard X12 rules that could be built in rather than requiring manual `IValidatable` code.
- **Conditional validation**: No way to express "validate element B only when element A has value X" declaratively. A conditional attribute would reduce manual validation code.
- **Validation message context**: Messages include segment/loop line numbers but not the segment tag or loop type name, making them harder to locate in the original EDI text.

### For Serialization
- **Segment separator in output**: The serializer always includes the trailing segment separator. Some trading partners expect no trailing separator on the last segment.
- **Whitespace/padding control**: ISA fields must be space-padded to exact lengths. There's no automatic padding -- users must ensure correct lengths manually.

### General
- **No NuGet package published yet**: The library is at version 0.1.0 with NuGet metadata configured but no published package.
- **EDIFACT support**: Only X12 is supported. EDIFACT (used internationally) would require a different separator scheme and segment structure.
- **Documentation**: XML doc comments exist on public APIs but not on generated code. IntelliSense for generated properties would help users explore the API.
