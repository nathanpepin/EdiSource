using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Loops;

public sealed class InterchangeEnvelope : ILoop<InterchangeEnvelope>, ISegmentIdentifier<InterchangeEnvelope>,
    ISegmentIdentifier<ISA>, ILoopInitialize<InterchangeEnvelope, InterchangeEnvelope>, ISourceGeneratorValidatable
{
    public static List<TransactionSetDefinition> TransactionSetDefinitions = [];

    public ISA ISA { get; set; } = default!;

    public LoopList<FunctionalGroup> FunctionalGroups { get; } = [];

    public ISegment IEA { get; set; } = default!;

    public InterchangeEnvelope? Parent => null;

    ILoop? ILoop.Parent => Parent;
    public List<IEdi?> EdiItems => [ISA, FunctionalGroups, IEA];

    public static Task<InterchangeEnvelope> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)
    {
        if (parent is null) return InitializeAsync(segmentReader, null);

        if (parent is not InterchangeEnvelope typedParent)
            throw new ArgumentException($"Parent must be of type {nameof(InterchangeEnvelope)}");

        return InitializeAsync(segmentReader, typedParent);
    }

    public static async Task<InterchangeEnvelope> InitializeAsync(ChannelReader<ISegment> segmentReader,
        InterchangeEnvelope? parent)
    {
        var loop = new InterchangeEnvelope();

        loop.ISA = await SegmentLoopFactory<ISA, InterchangeEnvelope>.CreateAsync(segmentReader, loop);

        while (await segmentReader.WaitToReadAsync())
        {
            if (await ISegmentIdentifier<FunctionalGroup>.MatchesAsync(segmentReader))
            {
                loop.FunctionalGroups.Add(await FunctionalGroup.InitializeAsync(segmentReader, loop));
                continue;
            }

            break;
        }

        loop.IEA = await SegmentLoopFactory<IEA, InterchangeEnvelope>.CreateAsync(segmentReader, loop);

        return loop;
    }

    public static (string Primary, string? Secondary) EdiId => ISA.EdiId;

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