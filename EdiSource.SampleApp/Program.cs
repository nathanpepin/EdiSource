using EdiSource.Domain;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.Standard.Segments;
using EdiSource.Domain.Validation.Data;
using _834 = EdiSource.Loops._834;
using Loop2000 = EdiSource.Loops.Loop2000;
using Loop2000_DTP = EdiSource.Segments.Loop2000_DTP;
using Loop2000_INS = EdiSource.Segments.Loop2000_INS;
using Loop2000_REF = EdiSource.Segments.Loop2000_REF;
using Loop2100 = EdiSource.Loops.Loop2100;
using Loop2100_DMG = EdiSource.Segments.Loop2100_DMG;
using Loop2100_N3 = EdiSource.Segments.Loop2100_N3;
using Loop2100_N4 = EdiSource.Segments.Loop2100_N4;
using Loop2100_NM1 = EdiSource.Segments.Loop2100_NM1;
using Loop2100_PER = EdiSource.Segments.Loop2100_PER;
using TS_DTP = EdiSource.Segments.TS_DTP;
using TS_REF = EdiSource.Segments.TS_REF;
using TS_SE = EdiSource.Segments.TS_SE;
using TS_ST = EdiSource.Segments.TS_ST;

Console.WriteLine("=============================================");
Console.WriteLine("EDI 834 BENEFIT ENROLLMENT PROCESSING DEMO");
Console.WriteLine("=============================================");

// Example 834 Benefit Enrollment transaction
var ediContent =
    """
    ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
    GS*BE*SENDER*RECEIVER*20230701*1200*1*X*005010X220A1~
    ST*834*0001~
    REF*38*123456789~
    DTP*356*D8*20230701~
    INS*Y*18*030*XN*A*E**FT~
    REF*0F*12345~
    DTP*356*D8*20230901~
    NM1*IL*1*DOE*JOHN*A***34*123456789~
    DMG*D8*19800101*M~
    N3*123 MAIN ST*APT 4B~
    N4*ANYTOWN*NY*12345~
    PER*IP*JANE DOE*TE*5555555555~
    NM1*IL*1*SMITH*SARAH*B***34*987654321~
    DMG*D8*19850215*F~
    N3*456 OAK AVE~
    N4*OTHERTOWN*CA*98765~
    SE*16*0001~
    GE*1*1~
    IEA*1*000000001~
    """;

// 1. Register 834 transaction set definition
Console.WriteLine("Registering 834 transaction set definition...");
InterchangeEnvelope.TransactionSetDefinitions.Add(_834.Definition);

// 2. Parse the EDI content into an interchange envelope
Console.WriteLine("Parsing 834 EDI content...");
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediContent);

// 3. Show the hierarchical structure
Console.WriteLine("\n--- 834 HIERARCHICAL STRUCTURE ---");
Console.WriteLine("Interchange Envelope");
Console.WriteLine($"  Sender: {envelope.ISA.InterchangeSenderId.Trim()}");
Console.WriteLine($"  Receiver: {envelope.ISA.InterchangeReceiverId.Trim()}");
Console.WriteLine($"  Control #: {envelope.ISA.InterchangeControlNumber}");
Console.WriteLine($"  Segment Separator: {separators.SegmentSeparator}");
Console.WriteLine($"  Data Element Separator: {separators.DataElementSeparator}");
Console.WriteLine($"  Composite Element Separator {separators.CompositeElementSeparator}");

var functionalGroup = envelope.FunctionalGroups[0];
Console.WriteLine("  Functional Group");
Console.WriteLine($"    Group Control #: {functionalGroup.GS.E06GroupControlNumber}");

// 4. Access the 834 transaction
var transaction = functionalGroup.TransactionSets[0] as _834;
if (transaction != null)
{
    Console.WriteLine("    834 Transaction");
    Console.WriteLine($"      Control #: {transaction.ST.TransactionSetControlNumber}");

    // 5. Access transaction-level data
    Console.WriteLine("\r\n--- TRANSACTION LEVEL DATA ---");
    Console.WriteLine($"Transaction Control #: {transaction.ST.TransactionSetControlNumber}");

    if (transaction.REFs.Count > 0)
    {
        Console.WriteLine("References:");
        foreach (var reference in transaction.REFs)
        {
            Console.WriteLine($"  {reference.ReferenceQualifier}: {reference.ReferenceId}");
        }
    }

    if (transaction.DTP != null)
    {
        Console.WriteLine($"Transaction Date ({transaction.DTP.DateQualifier}): {transaction.DTP.Date:yyyy-MM-dd}");
    }

    // 6. Access the 2000 loop (insured information)
    Console.WriteLine("\n--- INSURED LOOP (2000) DATA ---");
    var insured = transaction.Loop2000;
    Console.WriteLine($"Insured Indicator: {insured.INS.InsuredIndicator}");
    Console.WriteLine($"Relationship Code: {insured.INS.IndividualRelationshipCode}");

    if (insured.REFs.Count > 0)
    {
        Console.WriteLine("Insured References:");
        foreach (var reference in insured.REFs)
        {
            Console.WriteLine($"  {reference.ReferenceQualifier}: {reference.ReferenceId}");
        }
    }

    if (insured.DTPs.Count > 0)
    {
        Console.WriteLine("Insured Dates:");
        foreach (var date in insured.DTPs)
        {
            Console.WriteLine($"  {date.DateQualifier}: {date.Date:yyyy-MM-dd}");
        }
    }

    // 7. Access the 2100 loops (member information)
    Console.WriteLine("\n--- MEMBER LOOPS (2100) DATA ---");
    Console.WriteLine($"Total members: {transaction.Loop2100s.Count}");

    int memberCount = 0;
    foreach (var member in transaction.Loop2100s)
    {
        memberCount++;
        Console.WriteLine($"\nMember #{memberCount}:");
        Console.WriteLine($"  Name: {member.NM1.FirstName} {member.NM1.LastName}");

        if (member.Demographics.Count > 0)
        {
            var demographics = member.Demographics[0];
            Console.WriteLine($"  Birth Date: {demographics.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"  Gender: {demographics.Gender}");
        }

        if (member.Addresses.Count > 0)
        {
            var address = member.Addresses[0];
            Console.WriteLine($"  Address: {address.AddressLine1}");
            if (!string.IsNullOrEmpty(address.AddressLine2))
            {
                Console.WriteLine($"           {address.AddressLine2}");
            }
        }

        if (member.CityStateZips.Count > 0)
        {
            var csz = member.CityStateZips[0];
            Console.WriteLine($"  City/State/ZIP: {csz.City}, {csz.State} {csz.PostalCode}");
        }

        if (member.ContactInfo.Count > 0)
        {
            var contact = member.ContactInfo[0];
            Console.WriteLine($"  Contact: {contact.ContactName} ({contact.ContactFunction})");
            if (!string.IsNullOrEmpty(contact.CommunicationNumber1))
            {
                Console.WriteLine($"  {contact.CommunicationQualifier1}: {contact.CommunicationNumber1}");
            }
        }
    }

    // 8. Validate the document
    Console.WriteLine("\n--- VALIDATING 834 DOCUMENT ---");
    EdiValidationResult validationResult = EdiCommon.Validate(envelope);
    if (validationResult.IsValid)
    {
        Console.WriteLine("Validation passed successfully.");
    }
    else
    {
        Console.WriteLine("Validation failed with the following issues:");
        foreach (var message in validationResult.ValidationMessages)
        {
            Console.WriteLine($"  {message.Severity}: {message.Message}");
        }
    }

    // 9. Create a new 834 document programmatically
    Console.WriteLine("\n--- CREATING A NEW 834 DOCUMENT ---");

    var newEnvelope = new InterchangeEnvelope
    {
        ISA = ISA.CreateDefault(
            senderQualifier: "ZZ",
            senderId: "NEWSENDER      ",
            receiverQualifier: "ZZ",
            receiverId: "NEWRECEIVER    ",
            controlNumber: 12345,
            usageIndicator: "P"
        ),
        FunctionalGroups =
        [
            new FunctionalGroup
            {
                GS = new GS { E06GroupControlNumber = "54321" },
                TransactionSets =
                [
                    new _834
                    {
                        ST = new TS_ST { TransactionSetControlNumber = "9876" },
                        REFs =
                        [
                            new TS_REF
                            {
                                ReferenceQualifier = "38",
                                ReferenceId = "NEWPOLICY123"
                            }
                        ],
                        DTP = new TS_DTP
                        {
                            DateQualifier = "356",
                            DateFormatQualifier = "D8",
                            Date = DateTime.Now
                        },
                        Loop2000 = new Loop2000
                        {
                            INS = new Loop2000_INS
                            {
                                InsuredIndicator = "Y",
                                IndividualRelationshipCode = "18"
                            },
                            REFs =
                            [
                                new Loop2000_REF
                                {
                                    ReferenceQualifier = "0F",
                                    ReferenceId = "EMPID123456"
                                }
                            ],
                            DTPs =
                            [
                                new Loop2000_DTP
                                {
                                    DateQualifier = "356",
                                    DateFormatQualifier = "D8",
                                    Date = new DateTime(2023, 10, 1)
                                }
                            ]
                        },
                        Loop2100s =
                        [
                            new Loop2100
                            {
                                NM1 = new Loop2100_NM1
                                {
                                    EntityIdentifierCode = "IL",
                                    EntityTypeQualifier = "1",
                                    LastName = "JONES",
                                    FirstName = "MARY",
                                    MiddleName = "E"
                                },
                                Demographics =
                                [
                                    new Loop2100_DMG
                                    {
                                        DateOfBirth = new DateTime(1990, 5, 15),
                                        Gender = "F"
                                    }
                                ],
                                Addresses =
                                [
                                    new Loop2100_N3
                                    {
                                        AddressLine1 = "789 PINE STREET",
                                        AddressLine2 = "SUITE 101"
                                    }
                                ],
                                CityStateZips =
                                [
                                    new Loop2100_N4
                                    {
                                        City = "SOMECITY",
                                        State = "TX",
                                        PostalCode = "75001"
                                    }
                                ],
                                ContactInfo =
                                [
                                    new Loop2100_PER
                                    {
                                        ContactFunction = "IP",
                                        ContactName = "JONES, MARY",
                                        CommunicationQualifier1 = "TE",
                                        CommunicationNumber1 = "2145551234"
                                    }
                                ]
                            }
                        ],
                        SE = new TS_SE
                        {
                            SegmentCount = 14, // This would normally be calculated
                            TransactionSetControlNumber = "9876"
                        }
                    }
                ],
                GE = new GE
                {
                    E01NumberOfTransactionSets = 1,
                    E02GroupControlNumber = "54321"
                }
            }
        ],
        IEA = new IEA
        {
            E01NumberOfFunctionalGroups = 1,
            E02InterchangeControlNumber = "000012345"
        }
    };

    // 10. Serialize the new document
    Console.WriteLine("Serializing the new 834 document...");
    var serializedEdi = EdiCommon.WriteEdiToString(newEnvelope);
    Console.WriteLine("\n--- SERIALIZED 834 DOCUMENT ---");
    Console.WriteLine(serializedEdi);

    // 11. Modify the original document
    Console.WriteLine("\n--- MODIFYING THE ORIGINAL 834 DOCUMENT ---");
    // Change member information
    if (transaction.Loop2100s.Count > 0)
    {
        var firstMember = transaction.Loop2100s[0];
        Console.WriteLine($"Original name: {firstMember.NM1.FirstName} {firstMember.NM1.LastName}");
        firstMember.NM1.FirstName = "JONATHAN";
        Console.WriteLine($"Modified name: {firstMember.NM1.FirstName} {firstMember.NM1.LastName}");

        // Add a new reference at transaction level
        transaction.REFs.Add(new TS_REF
        {
            ReferenceQualifier = "ZZ",
            ReferenceId = "MODIFIED-REF"
        });

        // Add a new member
        transaction.Loop2100s.Add(new Loop2100
        {
            NM1 = new Loop2100_NM1
            {
                EntityIdentifierCode = "IL",
                EntityTypeQualifier = "1",
                LastName = "ADDISON",
                FirstName = "ALICE"
            },
            Demographics =
            [
                new Loop2100_DMG
                {
                    DateOfBirth = new DateTime(1995, 3, 20),
                    Gender = "F"
                }
            ]
        });

        Console.WriteLine($"Added new member: ALICE ADDISON");
        Console.WriteLine($"New member count: {transaction.Loop2100s.Count}");
    }

    // Serialize the modified document
    Console.WriteLine("\nSerializing the modified document...");
    var modifiedEdi = EdiCommon.WriteEdiToString(envelope);
    Console.WriteLine("\n--- MODIFIED 834 DOCUMENT ---");
    Console.WriteLine(modifiedEdi);
}

Console.WriteLine("\n=============================================");
Console.WriteLine("834 EDI PROCESSING DEMO COMPLETED");
Console.WriteLine("=============================================");