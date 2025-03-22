using EdiSource.Domain.Elements;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Segments;

public sealed class ISA : Segment, IEdi<InterchangeEnvelope>, ISegmentIdentifier<ISA>, ISourceGeneratorValidatable
{
    public InterchangeEnvelope? Parent { get; set; }
    public static EdiId EdiId { get; } = new("ISA");

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
        new BeTimeAttribute(ValidationSeverity.Critical, 10, 4),
        new ElementLengthAttribute(ValidationSeverity.Critical, 11, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 12, 5),
        new ElementLengthAttribute(ValidationSeverity.Critical, 13, 9),
        new ElementLengthAttribute(ValidationSeverity.Critical, 14, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 15, 1),
        new ElementLengthAttribute(ValidationSeverity.Critical, 16, 1)
    ];

    private const int SegmentExpectedElementCount = 16;


    public ISA()
    {
    }

    public ISA(Segment segment) : base(segment)
    {
        ValidateElementCount();
    }

    public ISA(string segmentText, Separators? separators = null) : base(segmentText, separators)
    {
        ValidateElementCount();
    }

    public ISA(IEnumerable<Element>? elements = null, Separators? separators = null) : base(elements, separators)
    {
        ValidateElementCount();
    }

    private void ValidateElementCount()
    {
        if (Elements.Count != SegmentExpectedElementCount)
        {
            throw new ArgumentException(
                $"ISA segment must contain exactly {SegmentExpectedElementCount} elements. Found {Elements.Count} elements.");
        }
    }

    private string ValidateAndPadField(string value, int requiredLength, string fieldName, bool throwIfInvalid = false)
    {
        if (value.Length <= requiredLength) return value.PadRight(requiredLength);
        
        if (!throwIfInvalid)
        {
            var trimmedValue = value.Trim();
            return trimmedValue.Length <= requiredLength 
                ? trimmedValue.PadRight(requiredLength) 
                : value[..requiredLength];
        }
            
        throw new ArgumentException(
            $"{fieldName} must be {requiredLength} characters or less. Found {value.Length} characters.");
    }

    // ISA-01: Authorization Information Qualifier (I01)
    public string AuthorizationInformationQualifier
    {
        get => this[1];
        set => this[1] = ValidateAndPadField(value, 2, "Authorization Information Qualifier");
    }

    // ISA-02: Authorization Information (I02)
    public string AuthorizationInformation
    {
        get => this[2];
        set => this[2] = ValidateAndPadField(value, 10, "Authorization Information");
    }

    // ISA-03: Security Information Qualifier (I03)
    public string SecurityInformationQualifier
    {
        get => this[3];
        set => this[3] = ValidateAndPadField(value, 2, "Security Information Qualifier");
    }

    // ISA-04: Security Information (I04)
    public string SecurityInformation
    {
        get => this[4];
        set => this[4] = ValidateAndPadField(value, 10, "Security Information");
    }

    // ISA-05: Interchange ID Qualifier (I05)
    public string InterchangeSenderQualifier
    {
        get => this[5];
        set => this[5] = ValidateAndPadField(value, 2, "Interchange Sender Qualifier");
    }

    // ISA-06: Interchange Sender ID (I06)
    public string InterchangeSenderId
    {
        get => this[6];
        set => this[6] = ValidateAndPadField(value, 15, "Interchange Sender ID");
    }

    // ISA-07: Interchange ID Qualifier (I05)
    public string InterchangeReceiverQualifier
    {
        get => this[7];
        set => this[7] = ValidateAndPadField(value, 2, "Interchange Receiver Qualifier");
    }

    // ISA-08: Interchange Receiver ID (I07)
    public string InterchangeReceiverId
    {
        get => this[8];
        set => this[8] = ValidateAndPadField(value, 15, "Interchange Receiver ID");
    }

    // ISA-09: Interchange Date (I08)
    public DateTime InterchangeDate
    {
        get => DateTime.ParseExact(this[9], "yyMMdd", null);
        set => this[9] = value.ToString("yyMMdd");
    }

    // ISA-10: Interchange Time (I09)
    public TimeSpan InterchangeTime
    {
        get => TimeSpan.ParseExact(this[10], "HHmm", null);
        set => this[10] = value.ToString("HHmm");
    }

    // ISA-11: Repetition Separator (I65)
    public char RepetitionSeparator
    {
        get => this[11][0];
        set => this[11] = value.ToString();
    }

    // ISA-12: Interchange Control Version Number (I11)
    public string InterchangeControlVersionNumber
    {
        get => this[12];
        set => this[12] = ValidateAndPadField(value, 5, "Interchange Control Version Number");
    }

    // ISA-13: Interchange Control Number (I12)
    public string InterchangeControlNumber
    {
        get => this[13];
        set => this[13] = value.PadLeft(9, '0');
    }

    // ISA-14: Acknowledgment Requested (I13)
    public string AcknowledgmentRequested
    {
        get => this[14];
        set => this[14] = ValidateAndPadField(value, 1, "Acknowledgment Requested");
    }

    // ISA-15: Usage Indicator (I14)
    public string UsageIndicator
    {
        get => this[15];
        set => this[15] = ValidateAndPadField(value, 1, "Usage Indicator");
    }

    // ISA-16: Component Element Separator (I15)
    public char ComponentElementSeparator
    {
        get => this[16][0];
        set => this[16] = value.ToString();
    }

    /// <summary>
    /// Creates a new ISA segment with default values
    /// </summary>
    public static ISA CreateDefault(
        string senderQualifier,
        string senderId,
        string receiverQualifier,
        string receiverId,
        int controlNumber,
        string usageIndicator = "P",
        string acknowledgmentRequested = "0",
        string version = "00501")
    {
        var elements = new List<Element>
        {
            new(["00"]), // ISA-01: Authorization Information Qualifier
            new(["          "]), // ISA-02: Authorization Information
            new(["00"]), // ISA-03: Security Information Qualifier
            new(["          "]), // ISA-04: Security Information
            new([senderQualifier]), // ISA-05: Interchange ID Qualifier
            new([senderId]), // ISA-06: Interchange Sender ID
            new([receiverQualifier]), // ISA-07: Interchange ID Qualifier
            new([receiverId]), // ISA-08: Interchange Receiver ID
            new([DateTime.Now.ToString("yyMMdd")]), // ISA-09: Interchange Date
            new([DateTime.Now.ToString("HHmm")]), // ISA-10: Interchange Time
            new(["^"]), // ISA-11: Repetition Separator
            new([version]), // ISA-12: Interchange Control Version Number
            new([controlNumber.ToString().PadLeft(9, '0')]), // ISA-13: Interchange Control Number
            new([acknowledgmentRequested]), // ISA-14: Acknowledgment Requested
            new([usageIndicator]), // ISA-15: Usage Indicator
            new([
                Separators.DefaultSeparators.CompositeElementSeparator.ToString()
            ]) // ISA-16: Component Element Separator
        };

        return new ISA(elements);
    }
}