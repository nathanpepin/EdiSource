using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator;

public static class Helper
{
    public static bool IsEdiAttribute(string? attributeName)
    {
        return attributeName switch
        {
            SegmentHeaderAttribute or SegmentHeader => true,
            SegmentAttribute or Segment => true,
            SegmentListAttribute or SegmentList => true,
            LoopAttribute or Loop => true,
            LoopListAttribute or LoopList => true,
            SegmentFooterAttribute or SegmentFooter => true,
            _ => false
        };
    }

    public static List<(string Name, string Attribute, IPropertySymbol Property)> OrderEdiItems(
        List<(string Name, string Attribute, IPropertySymbol Property)> ediItems)
    {
        return ediItems.OrderBy(item => item.Attribute switch
        {
            SegmentHeaderAttribute or SegmentHeader => 0,
            SegmentAttribute or Segment => 1,
            SegmentListAttribute or SegmentList => 2,
            LoopAttribute or Loop => 3,
            LoopListAttribute or LoopList => 4,
            SegmentFooterAttribute or SegmentFooter => 5,
            _ => 6
        }).ToList();
    }

    public static string GetEdiAttribute(IPropertySymbol property)
    {
        var attributes = property.GetAttributes();
        var ediAttribute = attributes.FirstOrDefault(a => IsEdiAttribute(a.AttributeClass?.Name));
        return ediAttribute?.AttributeClass?.Name ?? string.Empty;
    }
}