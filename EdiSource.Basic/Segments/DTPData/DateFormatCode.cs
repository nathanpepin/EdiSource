namespace EdiSource.Basic.Segments.DTPData;

public enum DateFormatCode
{
    CC, // First Two Digits of Year Expressed in Format CCYY
    CD, // Month and Year Expressed in Format MMMYYYY
    CM, // Date in Format CCYYMM
    CQ, // Date in Format CCYYQ
    CY, // Year Expressed in Format CCYY
    D6, // Date Expressed in Format YYMMDD
    D8, // Date Expressed in Format CCYYMMDD
    DA, // Range of Dates within a Single Month Expressed in Format DD-DD
    DB, // Date Expressed in Format MMDDCCYY
    DD, // Day of Month in Numeric Format
    DDT, // Range of Dates and Time, Expressed in CCYYMMDD-CCYYMMDDHHMM
    DT, // Date and Time Expressed in Format CCYYMMDDHHMM
    DTD, // Range of Dates and Time, Expressed in CCYYMMDDHHMM-CCYYMMDD
    DTS, // Range of Date and Time Expressed in Format CCYYMMDDHHMMSS-CCYYMMDDHHMMSS
    EH, // Last Digit of Year and Julian Date Expressed in Format YDDD
    KA, // Date Expressed in Format YYMMMDD
    MCY, // MMCCYY
    MD, // Month of Year and Day of Month Expressed in Format MMDD
    MM, // Month of Year in Numeric Format
    RD, // Range of Dates Expressed in Format MMDDCCYY-MMDDCCYY
    RD2, // Range of Years Expressed in Format YY-YY
    RD4, // Range of Years Expressed in Format CCYY-CCYY
    RD5, // Range of Years and Months Expressed in Format CCYYMM-CCYYMM
    RD6, // Range of Dates Expressed in Format YYMMDD-YYMMDD
    RD8, // Range of Dates Expressed in Format CCYYMMDD-CCYYMMDD
    RDM, // Range of Dates Expressed in Format YYMMDD-MMDD
    RDT, // Range of Date and Time, Expressed in Format CCYYMMDDHHMM-CCYYMMDDHHMM
    RMD, // Range of Months and Days Expressed in Format MMDD-MMDD
    RMY, // Range of Years and Months Expressed in Format YYMM-YYMM
    RTM, // Range of Time Expressed in Format HHMM-HHMM
    RTS, // Date and Time Expressed in Format CCYYMMDDHHMMSS
    TC, // Julian Date Expressed in Format DDD
    TM, // Time Expressed in Format HHMM
    TQ, // Date Expressed in Format MMYY
    TR, // Date and Time Expressed in Format DDMMYYHHMM
    TS, // Time Expressed in Format HHMMSS
    TT, // Date Expressed in Format MMDDYY
    TU, // Date Expressed in Format YYDDD
    UN, // Unstructured
    YM, // Year and Month Expressed in Format YYMM
    YMM, // Range of Year and Months, Expressed in CCYYMMM-MMM Format
    YY // Last Two Digits of Year Expressed in Format CCYY
}