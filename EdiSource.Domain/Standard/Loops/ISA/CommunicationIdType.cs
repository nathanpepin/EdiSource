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