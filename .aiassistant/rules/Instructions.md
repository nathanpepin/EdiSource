---
apply: always
---

# X12 Implementation Guide for AI Agent

## Phase 1: Analysis & Planning

### Extract from Stedi Guide:
1. **Transaction Identity**: ID (e.g., "271"), Version (e.g., "5010"), Implementation reference (e.g., "005010X279A1")
2. **Structure**: Find "Overview" section - extract all segments, loops, relationships, business meanings
3. **Discriminators**: Note HL03 values, NM101 codes, REF01 qualifiers, DTP01 qualifiers for loop disambiguation

### Project Structure Pattern:
```
EdiSource.{TransactionSet}-{Version}/  # e.g., EdiSource.271-5010
├── TransactionSet/
│   ├── _{TS}_{Ver}_{TransactionName}.cs
│   └── Segments/
│       ├── _{TS}_{Ver}_ST_TransactionSetHeader.cs
│       └── _{TS}_{Ver}_SE_TransactionSetTrailer.cs
├── Loop{ID}_{BusinessName}/
│   ├── _{TS}_{Ver}_Loop{ID}_{BusinessName}.cs
│   └── Segments/
│       └── _{TS}_{Ver}_Loop{ID}_{SegTag}_{BusinessName}.cs
└── Loop{Parent}/Loop{Child}_{BusinessName}/  # Nested loops
```

## Phase 2: Project Setup

### .csproj Template:
```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net9.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <RootNamespace>EdiSource._{TransactionSet}_{Version}</RootNamespace>
    </PropertyGroup>
    <ItemGroup>
        <ProjectReference Include="../EdiSource.Domain/EdiSource.Domain.csproj" />
        <ProjectReference Include="../EdiSource.Generator/EdiSource.Generator/EdiSource.Generator.csproj" 
                          OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
    </ItemGroup>
</Project>
```

### GlobalUsings.cs:
```csharp
global using EdiSource.Domain.SourceGeneration;
global using EdiSource.Domain.Validation.Data;
global using EdiSource.Domain.Validation.Factory;
global using EdiSource.Domain.Standard.Loops;
global using EdiSource.Domain.Loop;
global using EdiSource.Domain.Segments;
global using System.Collections.Generic;
global using static EdiSource.Domain.Identifiers.EdiId;
global using EdiSource._{TransactionSet}_{Version}.TransactionSet;
global using EdiSource._{TransactionSet}_{Version}.TransactionSet.Segments;
```

## Phase 3: Transaction Set Implementation

### Main Transaction Set Template:
```csharp
namespace EdiSource._{TS}_{Ver}.TransactionSet;

[LoopGenerator<FunctionalGroup, _{TS}_{Ver}_{TransactionName}, _{TS}_{Ver}_ST_TransactionSetHeader>]
public sealed partial class _{TS}_{Ver}_{TransactionName} : IValidatable, ITransactionSet<_{TS}_{Ver}_{TransactionName}>
{
    [SegmentHeader] public _{TS}_{Ver}_ST_TransactionSetHeader ST_TransactionSetHeader { get; set; } = null!;
    
    public _{TS}_{Ver}_{OptionalSegment}? {OptionalSegment}_{Description} { get; set; }
    [SegmentList] public SegmentList<_{TS}_{Ver}_{RepeatingSegment}> {RepeatingSegment}s_{Description} { get; set; } = [];
    
    public _{TS}_{Ver}_{SingleLoop}? {SingleLoop}_{Description} { get; set; }
    [LoopList] public LoopList<_{TS}_{Ver}_{RepeatingLoop}> {RepeatingLoop}s_{Description} { get; set; } = [];
    
    [SegmentFooter] public _{TS}_{Ver}_SE_TransactionSetTrailer SE_TransactionSetTrailer { get; set; } = null!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_{TS}_{Ver}_{TransactionName}>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        if (ST_TransactionSetHeader == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "ST segment is required");
        if (SE_TransactionSetTrailer == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "SE segment is required");
        // Add business validation rules
    }
}
```

### Segment Templates:

**ST Segment (with sub element identifiers):**
```csharp
[SegmentGenerator<_{TS}_{Ver}_{TransactionName}>("ST", "{TransactionID}", null, "{ImplementationRef}")]
public partial class _{TS}_{Ver}_ST_TransactionSetHeader
{
    public string? TransactionSetIdentifierCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? TransactionSetControlNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? ImplementationConventionReference { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
```

**Regular Segment with Type Conversions:**
```csharp
[SegmentGenerator<_{TS}_{Ver}_{TransactionName}>("{SegmentTag}")]
public partial class _{TS}_{Ver}_{SegmentTag}_{Description}
{
    public string? StringElement { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public int? IntElement { get => this.GetIntOptional(2); set => this.SetInt(value, 2); }
    public decimal? DecimalElement { get => this.GetDecimalOptional(3); set => this.SetDecimal(value, 3); }
    public DateTime? DateElement { get => this.GetDateTimeOptional(4, "yyyyMMdd"); set => this.SetDateTime(value, 4, "yyyyMMdd"); }
    public TimeSpan? TimeElement { get => this.GetTimeOptional(5, "HHmm"); set => this.SetTime(value, 5, "HHmm"); }
    public bool? BoolElement { get => this.GetBoolOptional(6); set => this.SetBool(value, 6); }
}
```

## Phase 4: Loop Implementation

### Loop Template:
```csharp
namespace EdiSource._{TS}_{Ver}.{LoopFolder};

[LoopGenerator<{ParentType}, _{TS}_{Ver}_{LoopName}, _{TS}_{Ver}_{HeaderSegment}>]
public partial class _{TS}_{Ver}_{LoopName} : IValidatable
{
    [SegmentHeader] public _{TS}_{Ver}_{HeaderSegment} {HeaderSegment}_{Description} { get; set; } = null!;
    
    public _{TS}_{Ver}_{OptionalSegment}? {OptionalSegment}_{Description} { get; set; }
    [SegmentList] public SegmentList<_{TS}_{Ver}_{RepeatingSegment}> {RepeatingSegment}s_{Description} { get; set; } = [];
    
    public _{TS}_{Ver}_{ChildLoop}? {ChildLoop}_{Description} { get; set; }
    [LoopList] public LoopList<_{TS}_{Ver}_{RepeatingChildLoop}> {RepeatingChildLoop}s_{Description} { get; set; } = [];
    
    public IEnumerable<ValidationMessage> Validate()
    {
        if ({HeaderSegment}_{Description} == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "{HeaderSegment} segment is required");
        // Add business validation
    }
}
```

### Loop Segment Template:
```csharp
namespace EdiSource._{TS}_{Ver}.{LoopFolder}.Segments;

[SegmentGenerator<_{TS}_{Ver}_{LoopName}>("{SegmentTag}", "{SubElementID1}", "{SubElementID2}", "{SubElementID3}")]
public partial class _{TS}_{Ver}_{LoopName}_{SegmentTag}_{Description}
{
    // Properties using appropriate types and positions
}
```

## Phase 5: Sub Element Identifiers

### Critical Patterns:

**Transaction Set:** `[SegmentGenerator<TransactionSet>("ST", "{TransactionID}", null, "{ImplementationRef}")]`

**Hierarchical Levels:** `[SegmentGenerator<Loop>("HL", null, null, "{HierarchicalCode}")]`
- HL03 = "20" (Information Source), "21" (Information Receiver), "22" (Subscriber), "23" (Dependent)

**Entity Identifiers:**
- Single: `[SegmentGenerator<Loop>("NM1", "{EntityCode}")]`
- Multiple: `[SegmentGenerator<Loop>("NM1", $"{Code1}{S}{Code2}{S}{Code3}")]`

**Reference Qualifiers:** `[SegmentGenerator<Loop>("REF", "{QualifierCode}")]`

**Date Qualifiers:** `[SegmentGenerator<Loop>("DTP", "{DateQualifier}")]`

### Common Entity Codes:
- "PR" (Payer), "1P" (Provider), "IL" (Subscriber), "03" (Dependent)
- Multiple Provider Types: `$"1P{S}2B{S}36{S}80{S}FA{S}GP{S}P5"`

### Multiple Values with S Separator:
```csharp
using static EdiSource.Domain.Identifiers.EdiId; // Provides S = "|"

[SegmentGenerator<Loop>("NM1", $"1P{S}2B{S}36{S}80")]
```

## Phase 6: LS/LE Bounded Loops

### When Needed:
- Multiple instances of same loop type could create parsing ambiguity
- Stedi guide shows LS/LE boundaries

### Pattern (requires TWO classes):

**Grouping Class:**
```csharp
[LoopGenerator<{ParentLoop}, _{TS}_{Ver}_{LoopName}Grouping, _{TS}_{Ver}_{LoopName}_LS_LoopStart>]
public partial class _{TS}_{Ver}_{LoopName}Grouping
{
    [SegmentHeader] public _{TS}_{Ver}_{LoopName}_LS_LoopStart? LS_LoopStart { get; set; }
    [LoopList] public LoopList<_{TS}_{Ver}_{LoopName}> {LoopName}s { get; set; } = [];
    [SegmentFooter] public _{TS}_{Ver}_{LoopName}_LE_LoopEnd? LE_LoopEnd { get; set; }
}
```

**Actual Loop Class:**
```csharp
[LoopGenerator<_{TS}_{Ver}_{LoopName}Grouping, _{TS}_{Ver}_{LoopName}, _{TS}_{Ver}_{HeaderSegment}>]
public partial class _{TS}_{Ver}_{LoopName} : IValidatable
{
    // Normal loop implementation
}
```

**LS/LE Segments:**
```csharp
[SegmentGenerator<_{TS}_{Ver}_{LoopName}Grouping>("LS", "{LoopID}")]
public partial class _{TS}_{Ver}_{LoopName}_LS_LoopStart
{
    public string? LoopIdentifierCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
}

[SegmentGenerator<_{TS}_{Ver}_{LoopName}Grouping>("LE", "{LoopID}")]
public partial class _{TS}_{Ver}_{LoopName}_LE_LoopEnd
{
    public string? LoopIdentifierCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
}
```

## Phase 7: Property Type Guidelines

### Type Mapping:
- **String/AN**: `string?`
- **Numeric/N**: `int?`, `decimal?` (based on precision needs)
- **Date**: `DateTime?` with format (e.g., "yyyyMMdd", "yyMMdd")
- **Time**: `TimeSpan?` with format (e.g., "HHmm", "HHmmss")
- **Boolean**: `bool?` for Y/N indicators
- **Code Values**: Consider enums for better type safety

### Property Rules:
- **Header/Footer segments**: Non-nullable with `= null!;`
- **All other segments**: Nullable
- **Single segments**: `SegmentType? SegmentName_Description { get; set; }`
- **Repeating segments**: `[SegmentList] public SegmentList<Type> Names_Description { get; set; } = [];`
- **Single loops**: `LoopType? LoopName_Description { get; set; }`
- **Repeating loops**: `[LoopList] public LoopList<Type> LoopNames_Description { get; set; } = [];`

## Phase 8: Validation Patterns

### Null-Safe Validation Template:
```csharp
public IEnumerable<ValidationMessage> Validate()
{
    // Critical: Header segments
    if (HeaderSegment == null)
        yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Header segment required");
    
    // Length validation (null-safe)
    if (!string.IsNullOrEmpty(Element) && Element.Length > MaxLength)
        yield return ValidationFactory.Create(this, ValidationSeverity.Error, $"Element exceeds max length {MaxLength}");
    
    // Conditional validation
    if (Element1 != null && Element2 == null)
        yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "Element2 recommended when Element1 present");
    
    // Date range validation
    if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Start date cannot be after end date");
    
    // Business rules with discriminator validation
    if (EntityIdentifierCode != "PR" && EntityIdentifierCode != "1P")
        yield return ValidationFactory.Create(this, ValidationSeverity.Error, "Invalid entity identifier code");
}
```

## Phase 9: Naming Conventions

### File Naming:
- **Transaction Set**: `_{TS}_{Ver}_{TransactionName}.cs`
- **Transaction Segments**: `_{TS}_{Ver}_{SegTag}_{Description}.cs`
- **Loop Classes**: `_{TS}_{Ver}_Loop{ID}_{Description}.cs`
- **Loop Segments**: `_{TS}_{Ver}_Loop{ID}_{SegTag}_{Description}.cs`
- **Grouping Classes**: `_{TS}_{Ver}_Loop{ID}_{Description}Grouping.cs`

### Folder Structure:
- **Transaction Set**: `TransactionSet/`
- **Transaction Segments**: `TransactionSet/Segments/`
- **Top-level Loops**: `Loop{ID}_{Description}/`
- **Loop Segments**: `Loop{ID}_{Description}/Segments/`
- **Nested Loops**: `Loop{Parent}_{Description}/Loop{Child}_{Description}/`
- **LS/LE Groups**: `Loop{Parent}/Loop{ID}_{Description}Grouping/Loop{ID}_{Description}/`

### Namespace Patterns:
- **Transaction Set**: `EdiSource._{TS}_{Ver}.TransactionSet`
- **Transaction Segments**: `EdiSource._{TS}_{Ver}.TransactionSet.Segments`
- **Loop**: `EdiSource._{TS}_{Ver}.Loop{ID}_{Description}`
- **Loop Segments**: `EdiSource._{TS}_{Ver}.Loop{ID}_{Description}.Segments`
- **Nested**: `EdiSource._{TS}_{Ver}.Loop{Parent}_{Description}.Loop{Child}_{Description}`

## Phase 10: Testing Template

### Basic Test Structure:
```csharp
[Fact]
public void CanParseBasic{TransactionSet}()
{
    var ediContent = """
        ISA*00*          *00*          *ZZ*SUBMITTER      *ZZ*RECEIVER       *250101*1023*^*00501*000000001*0*P*>~
        GS*{FunctionalCode}*SENDER*RECEIVER*20250101*1023*1*X*005010~
        ST*{TransactionID}*0001*{ImplementationRef}~
        {MinimalRequiredSegments}
        SE*{SegmentCount}*0001~
        GE*1*1~
        IEA*1*000000001~
        """;
    
    var result = EdiParser.Parse<_{TS}_{Ver}_{TransactionName}>(ediContent);
    
    Assert.NotNull(result);
    Assert.Equal("{TransactionID}", result.ST_TransactionSetHeader?.TransactionSetIdentifierCode);
    Assert.Equal("{ImplementationRef}", result.ST_TransactionSetHeader?.ImplementationConventionReference);
    // Test discriminator parsing
    // Test business logic
}

[Fact]
public void ValidatesRequiredElements()
{
    var transactionSet = new _{TS}_{Ver}_{TransactionName}();
    var validationResults = transactionSet.Validate().ToList();
    Assert.NotEmpty(validationResults);
    Assert.Contains(validationResults, v => v.Severity == ValidationSeverity.Critical);
}
```

## Key Implementation Rules

1. **Everything nullable except header/footer segments**
2. **Use descriptive business names, not technical segment names**
3. **Sub element identifiers are critical for parsing disambiguation**
4. **LS/LE loops need Grouping + Loop class pattern**
5. **Validation must be null-safe for real-world data**
6. **Folder structure mirrors X12 logical hierarchy**
7. **Use appropriate data types with conversion helpers**
8. **Multiple entity codes use `{S}` separator**
9. **Parent-child relationships through LoopGenerator attributes**
10. **Transaction set prefixing prevents naming conflicts**