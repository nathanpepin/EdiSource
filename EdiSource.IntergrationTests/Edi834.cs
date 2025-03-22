using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.IntergrationTests;

// Loop Models
[LoopGenerator<FunctionalGroup, Loop834, ST_834>]
public partial class Loop834 : ILoop
{
    [SegmentHeader] public ST_834 ST { get; set; } = default!;

    [SegmentList] public SegmentList<REF_834> REFs { get; set; } = [];

    [Loop] public Loop2000 InsuredLoop { get; set; } = default!;

    [LoopList] public LoopList<Loop2100> MemberLoops { get; set; } = [];

    [SegmentFooter] public SE_834 SE { get; set; } = default!;
}

// Represents an insured person loop (2000)
[LoopGenerator<Loop834, Loop2000, INS_2000>]
public partial class Loop2000
{
    [SegmentHeader] public INS_2000 INS { get; set; } = default!;

    [SegmentList] public SegmentList<REF_2000> REFs { get; set; } = [];

    [SegmentList] public SegmentList<DTP_2000> DTPs { get; set; } = [];
}

// Represents a member detail loop (2100)
[LoopGenerator<Loop834, Loop2100, NM1_2100>]
public partial class Loop2100
{
    [SegmentHeader] public NM1_2100 NM1 { get; set; } = default!;

    [SegmentList] public SegmentList<DMG_2100> Demographics { get; set; } = [];

    [SegmentList] public SegmentList<N3_2100> Addresses { get; set; } = [];

    [SegmentList] public SegmentList<N4_2100> CityStateZips { get; set; } = [];

    [SegmentList] public SegmentList<PER_2100> ContactInfo { get; set; } = [];
}

// Segment Models
[SegmentGenerator<Loop834>("ST", "834")]
public partial class ST_834 : Segment
{
    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop834>("SE")]
public partial class SE_834 : Segment
{
    public int SegmentCount
    {
        get => this.GetIntRequired(1);
        set => this.SetInt(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2000>("INS")]
public partial class INS_2000 : Segment
{
    public string InsuredIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string IndividualRelationshipCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop834>("REF")]
public partial class REF_834 : Segment
{
    public string ReferenceQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ReferenceId
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2000>("REF")]
public partial class REF_2000 : Segment
{
    public string ReferenceQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ReferenceId
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2000>("DTP")]
public partial class DTP_2000 : Segment
{
    public string DateQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DateFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public DateTime Date
    {
        get => this.GetDateRequired(3);
        set => this.SetDate(value, 3);
    }
}

[SegmentGenerator<Loop2100>("NM1")]
public partial class NM1_2100 : Segment
{
    public string EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string EntityTypeQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string LastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string FirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}

[SegmentGenerator<Loop2100>("DMG")]
public partial class DMG_2100 : Segment
{
    public DateTime DateOfBirth
    {
        get => this.GetDateRequired(2);
        set => this.SetDate(value, 2);
    }

    public string Gender
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100>("N3")]
public partial class N3_2100 : Segment
{
    public string AddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? AddressLine2
    {
        get => GetCompositeElementOrNull(2);
        set => SetCompositeElement(value!, 2);
    }
}

[SegmentGenerator<Loop2100>("N4")]
public partial class N4_2100 : Segment
{
    public string City
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string State
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string PostalCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100>("PER")]
public partial class PER_2100 : Segment
{
    public string ContactFunction
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? ContactName
    {
        get => GetCompositeElementOrNull(2);
        set => SetCompositeElement(value!, 2);
    }

    public string? CommunicationQualifier1
    {
        get => GetCompositeElementOrNull(3);
        set => SetCompositeElement(value!, 3);
    }

    public string? CommunicationNumber1
    {
        get => GetCompositeElementOrNull(4);
        set => SetCompositeElement(value!, 4);
    }
}