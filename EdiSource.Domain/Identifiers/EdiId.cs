using System.Text;
using EdiSource.Domain.Elements;
using EdiSource.Domain.Segments;

namespace EdiSource.Domain.Identifiers;

public readonly struct EdiId(params Element?[] ids)
{
    private Element?[] Ids { get; } = ids;

    public bool MatchesSegment(ISegment segment)
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


    public override string ToString()
    {
        StringBuilder output = new();

        foreach (var de in Ids)
        {
            output.Append(de is null ? "" : string.Join(":", de));
            output.Append("*");
        }

        return output.ToString().TrimEnd('*');
    }
}