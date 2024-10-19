using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Segments;

public sealed class ISA : Segment, ISegment<InterchangeEnvelope>, ISegmentIdentifier<ISA>, ISourceGeneratorValidatable
{
    public new InterchangeEnvelope? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("ISA", null);
    
    public List<IIndirectValidatable> SourceGenValidations { get; } =
    [
        new RequiredDataElementsAttribute(ValidationSeverity.Critical,
            [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]),
        new ElementLengthAttribute(ValidationSeverity.Critical, 0, 3),
        new ElementLengthAttribute(ValidationSeverity.Critical, 1, 2),
        new IsOneOfValuesAttribute(ValidationSeverity.Error, 1, 0, "00", "01", "02", "03", "04", "05", "06", "07",
            "08"),
        new ElementLengthAttribute(ValidationSeverity.Critical, 2, 10),
        new ElementLengthAttribute(ValidationSeverity.Critical, 3, 2),
        new IsOneOfValuesAttribute(ValidationSeverity.Error, 3, 0, "00", "01"),
        new ElementLengthAttribute(ValidationSeverity.Critical, 4, 10),
        new ElementLengthAttribute(ValidationSeverity.Critical, 5, 2),
        IdentificationCodeExtensions.CreateIdentificationCodeAttribute(),
        new ElementLengthAttribute(ValidationSeverity.Critical, 6, 15),
        new ElementLengthAttribute(ValidationSeverity.Critical, 7, 2),
        new ElementLengthAttribute(ValidationSeverity.Critical, 8, 15),
        new ElementLengthAttribute(ValidationSeverity.Critical, 9, 6),
        new BeDateAttribute(ValidationSeverity.Critical, 9, 0, "yyMMdd"),
        new ElementLengthAttribute(ValidationSeverity.Critical, 10, 4),
        new BeTimeAttribute(ValidationSeverity.Critical, 10, 4, "HHmm"),
        new ElementLengthAttribute(ValidationSeverity.Critical, 11, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 12, 5),
        new ElementLengthAttribute(ValidationSeverity.Critical, 13, 9),
        new ElementLengthAttribute(ValidationSeverity.Critical, 14, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 15, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 16, 1),
    ];
}