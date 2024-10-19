using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;

namespace EdiSource.Domain.Standard.Loops;

public enum IdentificationCode
{
    Duns = 01,
    SCAC = 02,
    FMC = 03,
    IATA = 04,
    GLN = 07,
    UCCEDICommID = 08,
    X121 = 09,
    DoDActivityAddressCode = 10,
    DEA = 11,
    Phone = 12,
    UCSCode = 13,
    DunsPlusSuffix = 14,
    PetroleumAccountantsSocietyOfCanadaCode = 15,
    DunsNumberWith4CharSuffix = 16,
    ABARoutingNumber = 17,
    AARStandardDistributionCode = 18,
    EDICACommIDNumber = 19,
    HealthIndustryNumber = 20,
    IPEDS = 21,
    FICE = 22,
    NCESCommonCoreNumber = 23,
    CollegeBoardATPCode = 24,
    ACTCode = 25,
    StatisticsCanadaPostsecondaryList = 26,
    CMSCarrierIdentificationNumber = 27,
    CMSFiscalIntermediaryIdentificationNumber = 28,
    CMSMedicareProviderSupplierNumber = 29,
    USFederalTaxIdentificationNumber = 30,
    IAIABCJurisdictionIdentificationNumberPlus4 = 31,
    USFEIN = 32,
    NAICCompanyCode = 33,
    MedicaidProviderSupplierNumber = 34,
    StatisticsCanadaCollegeStudentInfoSystemCode = 35,
    StatisticsCanadaUniversityStudentInfoSystemCode = 36,
    SocietyOfPropertyInfoCompilersAndAnalysts = 37,
    CollegeBoardACTSecondaryInstitutionCode = 38,
    AMECOPCommunicationID = 39,
    NRMAAssigned = 40,
    SAFERUserIdentificationNumber = 41,
    StandardAddressNumber = 42,
    MutuallyDefined = 43
}

public static class IdentificationCodeExtensions
{
    public static string? EnumToString(this IdentificationCode value)
    {
        return value switch
        {
            IdentificationCode.Duns => "01",
            IdentificationCode.SCAC => "02",
            IdentificationCode.FMC => "03",
            IdentificationCode.IATA => "04",
            IdentificationCode.GLN => "07",
            IdentificationCode.UCCEDICommID => "08",
            IdentificationCode.X121 => "09",
            IdentificationCode.DoDActivityAddressCode => "10",
            IdentificationCode.DEA => "11",
            IdentificationCode.Phone => "12",
            IdentificationCode.UCSCode => "13",
            IdentificationCode.DunsPlusSuffix => "14",
            IdentificationCode.PetroleumAccountantsSocietyOfCanadaCode => "15",
            IdentificationCode.DunsNumberWith4CharSuffix => "16",
            IdentificationCode.ABARoutingNumber => "17",
            IdentificationCode.AARStandardDistributionCode => "18",
            IdentificationCode.EDICACommIDNumber => "19",
            IdentificationCode.HealthIndustryNumber => "20",
            IdentificationCode.IPEDS => "21",
            IdentificationCode.FICE => "22",
            IdentificationCode.NCESCommonCoreNumber => "23",
            IdentificationCode.CollegeBoardATPCode => "24",
            IdentificationCode.ACTCode => "25",
            IdentificationCode.StatisticsCanadaPostsecondaryList => "26",
            IdentificationCode.CMSCarrierIdentificationNumber => "27",
            IdentificationCode.CMSFiscalIntermediaryIdentificationNumber => "28",
            IdentificationCode.CMSMedicareProviderSupplierNumber => "29",
            IdentificationCode.USFederalTaxIdentificationNumber => "30",
            IdentificationCode.IAIABCJurisdictionIdentificationNumberPlus4 => "31",
            IdentificationCode.USFEIN => "32",
            IdentificationCode.NAICCompanyCode => "33",
            IdentificationCode.MedicaidProviderSupplierNumber => "34",
            IdentificationCode.StatisticsCanadaCollegeStudentInfoSystemCode => "35",
            IdentificationCode.StatisticsCanadaUniversityStudentInfoSystemCode => "36",
            IdentificationCode.SocietyOfPropertyInfoCompilersAndAnalysts => "37",
            IdentificationCode.CollegeBoardACTSecondaryInstitutionCode => "38",
            IdentificationCode.AMECOPCommunicationID => "AM",
            IdentificationCode.NRMAAssigned => "NR",
            IdentificationCode.SAFERUserIdentificationNumber => "SA",
            IdentificationCode.StandardAddressNumber => "SN",
            IdentificationCode.MutuallyDefined => "ZZ",
            _ => null
        };
    }

    public static IdentificationCode? StringToEnum(this string? value)
    {
        return value switch
        {
            "01" => IdentificationCode.Duns,
            "02" => IdentificationCode.SCAC,
            "03" => IdentificationCode.FMC,
            "04" => IdentificationCode.IATA,
            "07" => IdentificationCode.GLN,
            "08" => IdentificationCode.UCCEDICommID,
            "09" => IdentificationCode.X121,
            "10" => IdentificationCode.DoDActivityAddressCode,
            "11" => IdentificationCode.DEA,
            "12" => IdentificationCode.Phone,
            "13" => IdentificationCode.UCSCode,
            "14" => IdentificationCode.DunsPlusSuffix,
            "15" => IdentificationCode.PetroleumAccountantsSocietyOfCanadaCode,
            "16" => IdentificationCode.DunsNumberWith4CharSuffix,
            "17" => IdentificationCode.ABARoutingNumber,
            "18" => IdentificationCode.AARStandardDistributionCode,
            "19" => IdentificationCode.EDICACommIDNumber,
            "20" => IdentificationCode.HealthIndustryNumber,
            "21" => IdentificationCode.IPEDS,
            "22" => IdentificationCode.FICE,
            "23" => IdentificationCode.NCESCommonCoreNumber,
            "24" => IdentificationCode.CollegeBoardATPCode,
            "25" => IdentificationCode.ACTCode,
            "26" => IdentificationCode.StatisticsCanadaPostsecondaryList,
            "27" => IdentificationCode.CMSCarrierIdentificationNumber,
            "28" => IdentificationCode.CMSFiscalIntermediaryIdentificationNumber,
            "29" => IdentificationCode.CMSMedicareProviderSupplierNumber,
            "30" => IdentificationCode.USFederalTaxIdentificationNumber,
            "31" => IdentificationCode.IAIABCJurisdictionIdentificationNumberPlus4,
            "32" => IdentificationCode.USFEIN,
            "33" => IdentificationCode.NAICCompanyCode,
            "34" => IdentificationCode.MedicaidProviderSupplierNumber,
            "35" => IdentificationCode.StatisticsCanadaCollegeStudentInfoSystemCode,
            "36" => IdentificationCode.StatisticsCanadaUniversityStudentInfoSystemCode,
            "37" => IdentificationCode.SocietyOfPropertyInfoCompilersAndAnalysts,
            "38" => IdentificationCode.CollegeBoardACTSecondaryInstitutionCode,
            "AM" => IdentificationCode.AMECOPCommunicationID,
            "NR" => IdentificationCode.NRMAAssigned,
            "SA" => IdentificationCode.SAFERUserIdentificationNumber,
            "SN" => IdentificationCode.StandardAddressNumber,
            "ZZ" => IdentificationCode.MutuallyDefined,
            _ => null
        };
    }
    
    public static IsOneOfValuesAttribute CreateIdentificationCodeAttribute()
    {
        var validCodes = Enum.GetValues(typeof(IdentificationCode))
            .Cast<IdentificationCode>()
            .Select(code => code.EnumToString())
            .Where(code => code is { Length: 2 })
            .OfType<string>()
            .ToArray();

        return new IsOneOfValuesAttribute(ValidationSeverity.Error, 5, 0, validCodes);
    }
}