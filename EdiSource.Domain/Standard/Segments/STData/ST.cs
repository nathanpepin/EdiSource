using EdiSource.Domain.Helper;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Segments.STData;

public partial class Generic_ST : ST, ISegment<GenericTransactionSet>, ISegmentIdentifier<Generic_ST>
{
    public new GenericTransactionSet? Parent { get; set; }
}

public partial class ST : Segment, ISegmentIdentifier<ST>, ISourceGeneratorValidatable
{
    public static (string Primary, string? Secondary) EdiId { get; } = ("ST", null);

    public TransactionSetIdentifierCode TransactionSetIdentifierCode
    {
        get => this.GetEnumRequired<TransactionSetIdentifierCode>(1);
        set => this.SetEnum(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? ImplementationConventionReference
    {
        get => GetCompositeElementOrNull(3);
        set => value.DoIfNotNull(x => SetCompositeElement(x, 3));
    }

    public List<IIndirectValidatable> SourceGenValidations { get; } =
    [
        new RequiredDataElementsAttribute(ValidationSeverity.Critical, [0, 1, 2]),
        new IsOneOfValuesAttribute(ValidationSeverity.Critical, 0, 0, "ST"),
        new IsOneOfValuesAttribute(ValidationSeverity.Critical, 1, 0,
            EnumExtensions.EnumToStringArray<TransactionSetIdentifierCode>())
    ];
}