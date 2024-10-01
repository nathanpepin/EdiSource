namespace EdiSource.Domain.Standard.Loops.ISA;

public enum CommunicationIdType
{
    NoAuthorizationInfo = 00,
    UCSCommunicationsId = 01,
    EDXCommunicationsId = 02,
    AdditionalDataIdentification = 03,
    RailCommunicationsId = 04,
    DoDCommunicationId = 05,
    USFederalGovtCommunicationId = 06,
    TruckCommunicationsId = 07,
    OceanCommunicationsId = 08
}

public static class CommunicationIdTypeExtensions
{
    public static string? EnumToString(this CommunicationIdType value)
    {
        return value switch
        {
            CommunicationIdType.NoAuthorizationInfo => "00",
            CommunicationIdType.UCSCommunicationsId => "01",
            CommunicationIdType.EDXCommunicationsId => "02",
            CommunicationIdType.AdditionalDataIdentification => "03",
            CommunicationIdType.RailCommunicationsId => "04",
            CommunicationIdType.DoDCommunicationId => "05",
            CommunicationIdType.USFederalGovtCommunicationId => "06",
            CommunicationIdType.TruckCommunicationsId => "07",
            CommunicationIdType.OceanCommunicationsId => "08",
            _ => null
        };
    }

    public static CommunicationIdType? StringToEnum(this string? value)
    {
        return value switch
        {
            "00" => CommunicationIdType.NoAuthorizationInfo,
            "01" => CommunicationIdType.UCSCommunicationsId,
            "02" => CommunicationIdType.EDXCommunicationsId,
            "03" => CommunicationIdType.AdditionalDataIdentification,
            "04" => CommunicationIdType.RailCommunicationsId,
            "05" => CommunicationIdType.DoDCommunicationId,
            "06" => CommunicationIdType.USFederalGovtCommunicationId,
            "07" => CommunicationIdType.TruckCommunicationsId,
            "08" => CommunicationIdType.OceanCommunicationsId,
            _ => null
        };
    }
}