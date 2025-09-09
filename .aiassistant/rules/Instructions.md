---
apply: always
---

# EdiSource X12 Implementation Guide

## Project Context

EdiSource is a high-performance C# library for processing EDI (Electronic Data Interchange) documents, specifically X12 standards. Key principles:

- **Performance-focused**: Memory efficiency, async processing with Channels
- **Domain-driven design** with source generation to reduce boilerplate
- **Functional programming** where natural (immutable data, pure functions)
- **Modern .NET 9** features (nullable references, records, pattern matching)
- **X12-aligned naming**: Conventions match X12 specs rather than .NET standards

## Prerequisites

- Stedi implementation guide PDF for target transaction set
- Understanding of X12 concepts (segments, loops, elements)
- Access to EdiSource project patterns

## Phase 1: Analysis

### 1.1 Extract from Stedi Guide

**Transaction Identity:**
- Set ID (e.g., "999", "271", "837P")
- Version (e.g., "5010")
- Implementation reference (e.g., "005010X231")

**Structure Map:**
```
Transaction Set
├── ST (Header)
├── [Root Segments]
├── Loop XXXX
│   ├── Header Segment
│   └── Nested Loop YYYY
└── SE (Footer)
```

### 1.2 Project Structure

**Naming:** `EdiSource.{TransactionSet}-{Version}` (e.g., `EdiSource.999-5010`)

**Folder Organization:**
```
EdiSource.{TransactionSet}-{Version}/
├── _{TransactionSet}.cs             // Main transaction set
├── Segments/                        // Transaction-level segments only
│   ├── ST.cs
│   ├── SE.cs
│   └── [root-level segments]
├── Loops/                          // All loops with context-specific segments
│   ├── Loop1000A.cs                // Loop structure
│   ├── Loop1000A.NM1.cs           // Loop-specific segments
│   └── [all loops and their segments]
├── EdiSource.{TransactionSet}-{Version}.csproj
└── GlobalUsings.cs
```

**Key Principle:** Each loop owns its segments completely. No shared/common segments due to context-dependent validation.

## Phase 2: Setup

### 2.1 Project File

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
                          OutputItemType="Analyzer" 
                          ReferenceOutputAssembly="false" />
    </ItemGroup>
</Project>
```

### 2.2 Global Usings

```csharp
global using EdiSource.Domain.SourceGeneration;
global using EdiSource.Domain.Validation.Data;
global using EdiSource.Domain.Validation.Factory;
global using EdiSource.Domain.Standard.Loops;
global using EdiSource.Domain.Loop;
global using EdiSource.Domain.Segments;
global using System.Collections.Generic;
```

## Phase 3: Transaction Set Implementation

### 3.1 Main Transaction Set Class

```csharp
namespace EdiSource._{TransactionSet}_{Version}.Loops;

[LoopGenerator<FunctionalGroup, _{TransactionSet}, TS{TransactionSet}_ST>]
public sealed partial class _{TransactionSet} : IValidatable, ITransactionSet<_{TransactionSet}>
{
    [SegmentHeader] public TS{TransactionSet}_ST ST { get; set; } = null!;
    
    // Root-level segments/loops (see property mapping below)
    
    [SegmentFooter] public TS{TransactionSet}_SE SE { get; set; } = null!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_{TransactionSet}>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        // Validation logic
        yield break;
    }
}
```

**Property Mapping:**
- Required single: `[Segment] public Type Name { get; set; } = null!;`
- Optional single: `[Segment] public Type? Name { get; set; }`
- Repeating: `[SegmentList] public SegmentList<Type> Names { get; set; } = [];`
- Loop: `[Loop]` or `[LoopList]` with same patterns

### 3.2 Transaction Segments

```csharp
[SegmentGenerator<_{TransactionSet}>("ST")]
public partial class TS{TransactionSet}_ST
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
    
    public string ImplementationConventionReference
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}
```

## Phase 4: Loop Implementation

### 4.1 Loop Class Template

```csharp
[LoopGenerator<{ParentType}, Loop{ID}, Loop{ID}_{HeaderSegmentID}>]
public partial class Loop{ID} : IValidatable
{
    [SegmentHeader] public Loop{ID}_{HeaderSegmentID} {HeaderSegmentID} { get; set; } = null!;
    
    // Loop content using same property mapping rules
    
    public IEnumerable<ValidationMessage> Validate()
    {
        return [];
    }
}
```

**Parent Type:**
- Direct under transaction: `_{TransactionSet}`
- Nested under loop: `Loop{ParentID}`

### 4.2 Loop Segments

```csharp
[SegmentGenerator<Loop{ID}>("{SegmentID}")]
public partial class Loop{ID}_{SegmentID}
{
    public string {ElementName}
    {
        get => GetCompositeElement({Position});
        set => SetCompositeElement(value, {Position});
    }
    
    // For numeric elements:
    // public int {ElementName}
    // {
    //     get => this.GetIntRequired({Position});
    //     set => this.SetInt(value, {Position});
    // }
}
```

## Phase 5: Special Cases - LS/LE Bounded Loops

Some loops require LS/LE boundaries to prevent parsing ambiguity. This requires a "Grouping" pattern.

### Structure

```
Loop2110D (parent)
├── Loop2120DGrouping (boundary handler)
│   ├── LS (start)
│   ├── Multiple Loop2120D instances
│   └── LE (end)
```

### Implementation

**Grouping Class:**
```csharp
[LoopGenerator<Loop2110D, Loop2120DGrouping, Loop2120D_LS>]
public partial class Loop2120DGrouping
{
    [SegmentHeader] public Loop2120D_LS? LS { get; set; }
    [LoopList] public LoopList<Loop2120D> Loop2120Ds { get; set; } = [];
    [SegmentFooter] public Loop2120D_LE? LE { get; set; }
}
```

**Actual Loop:**
```csharp
[LoopGenerator<Loop2120DGrouping, Loop2120D, Loop2120D_NM1>]
public partial class Loop2120D : IValidatable
{
    // Normal loop implementation
}
```

**Parent Integration:**
```csharp
// Parent references the Grouping, not individual loop
[Loop] public Loop2120DGrouping? Loop2120DGrouping { get; set; }
```

## Phase 6: Validation

```csharp
public IEnumerable<ValidationMessage> Validate()
{
    // Required elements
    if (RequiredProperty == null!)
        yield return ValidationFactory.Create(this, ValidationSeverity.Critical, 
            "Required element [{ElementName}] is missing");
    
    // Business rules from Stedi guide
    // Length validation
    // Cross-reference validation
}
```

## Phase 7: Testing

Create `EdiSource.{TransactionSet}-{Version}.Tests`:

```csharp
[Fact]
public void CanParseBasic{TransactionSet}()
{
    var ediContent = "ISA*...\r\nGS*...\r\nST*{TransactionSet}*0001*{Ref}~\r\n...\r\nSE*...\r\nGE*...\r\nIEA*...\r\n";
    var result = EdiParser.Parse(ediContent);
    Assert.NotNull(result);
}
```

## Naming Conventions

- **Namespace:** `EdiSource._{TransactionSet}_{Version}.Loops`
- **Transaction Set:** `_{TransactionSet}`
- **Transaction Segments:** `TS{TransactionSet}_{SegmentID}`
- **Loops:** `Loop{ID}`
- **Loop Segments:** `Loop{ID}_{SegmentID}`
- **Grouping:** `Loop{ID}Grouping`

## Generator Attributes

- **Transaction Set:** `[LoopGenerator<FunctionalGroup, Self, HeaderSegment>]`
- **Loop:** `[LoopGenerator<Parent, Self, HeaderSegment>]`
- **Segment:** `[SegmentGenerator<Parent>("ID")]`
- **Properties:** `[Segment]`, `[SegmentList]`, `[Loop]`, `[LoopList]`
- **Special:** `[SegmentHeader]`, `[SegmentFooter]`

## Quality Checklist

- [ ] All segments from Stedi guide implemented
- [ ] Loop structure matches Overview exactly
- [ ] Property names use PascalCase
- [ ] Required/optional attributes correct
- [ ] Generator attributes have correct parameters
- [ ] Parent relationships accurate
- [ ] Validation covers critical rules
- [ ] Follows established patterns

## Handling Ambiguity

- **Missing details:** Use conservative string type
- **Unclear relationships:** Follow Overview structure
- **Optional vs Required:** Check "Usage notes" and "Max use"
- **Conflicts:** Document in comments, use Overview as authoritative

## Comprehensive Segment Validation Example

```csharp
[SegmentGenerator<_837P_Loop2000A>("HL")]
public partial class _837P_Loop2000A_HL : ISourceGeneratorValidatable
{
    public string HierarchicalIdNumber
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
    
    public HierarchicalLevelCodeType HierarchicalLevelCode
    {
        get => this.GetEnumRequired<HierarchicalLevelCodeType>(3);
        set => this.SetEnum(value, 3);
    }
    
    public List<IIndirectValidatable> SourceGenValidations { get; } =
    [
        new RequiredDataElementsAttribute(ValidationSeverity.Critical, [0, 1, 3]),
        new ElementLengthAttribute(ValidationSeverity.Critical, 1, 1, 12),
        new BeIntAttribute(ValidationSeverity.Critical, 1, 0),
        new IsOneOfValuesAttribute(ValidationSeverity.Critical, 3, 0, "20", "21", "22", "23")
    ];
}
```