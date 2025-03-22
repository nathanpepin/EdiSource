using System.Text;
using EdiSource.Domain.Elements;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Represents an ID of a segment.
///     If a value in the elements is null, it is ignored for matching.
/// </summary>
/// <param name="ids"></param>
public readonly struct EdiId(params Element?[] ids)
{
    private Element?[] Ids { get; } = ids;

    public bool MatchesSegment(Segment segment)
    {
        for (var deI = 0; deI < Ids.Length; deI++)
        {
            var de = Ids[deI];
            if (de is null) continue;

            for (var ceI = 0; ceI < de.Count; ceI++)
            {
                var ce = de[ceI];

                if (segment.GetCompositeElementOrNull(deI, ceI) is not { } value || value != ce)
                    return false;
            }
        }

        return true;
    }

    public bool MatchesEdiId(EdiId other)
    {
        if (Ids.Length != other.Ids.Length) return false;

        for (var deI = 0; deI < Ids.Length; deI++)
        {
            var de = Ids[deI];
            var deOther = other.Ids[deI];

            if (de is null && deOther is null) continue;
            if (de is null || deOther is null) return false;
            if (de.Count != deOther.Count) return false;

            for (var ceI = 0; ceI < de.Count; ceI++)
            {
                var ce = de[ceI];
                var ceOther = deOther[ceI];

                if (string.IsNullOrEmpty(ce) && string.IsNullOrEmpty(ceOther)) continue;
                if (ce.Length != ceOther.Length) return false;
                if (ce != ceOther) return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Copies elements from an EdiId to a segment.
    /// Useful in cases where you want to create a segment subtype without having to specify the element values. 
    /// </summary>
    /// <param name="segment"></param>
    public void CopyIdElementsToSegment(Segment segment)
    {
        for (var dei = 0; dei < Ids.Length; dei++)
        {
            var de = Ids[dei];
            if (de is not { Count: > 0 }) continue;

            for (var cei = 0; cei < de.Count; cei++)
            {
                var ce = de[cei];
                segment.SetCompositeElement(ce, dei, cei);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder output = new();

        foreach (var de in Ids)
        {
            output.Append(de is null ? "" : string.Join(":", de.Cast<string>()));
            output.Append('*');
        }

        return output.ToString().TrimEnd('*');
    }
}