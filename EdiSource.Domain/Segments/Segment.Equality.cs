// namespace EdiSource.Domain.Segments;
//
// public partial class Segment
// {
//     public override bool Equals(object? obj)
//     {
//         if (obj is not Segment segment) return false;
//         return Equals(segment);
//     }
//
//     public bool Equals(Segment other)
//     {
//         if (other.Elements.Count != Elements.Count) return false;
//
//         foreach (var (e, x) in other.Elements.Zip(Elements, (s, s1) => (s, s1)))
//             if (!e.Equals(x))
//                 return false;
//
//         return true;
//     }
//
//     public static bool operator ==(Segment left, Segment right)
//     {
//         return Equals(left, right);
//     }
//
//     public static bool operator !=(Segment element1, Segment element2)
//     {
//         return !(element1 == element2);
//     }
//
//     public override int GetHashCode()
//     {
//         return HashCode.Combine(this);
//     }
// }

