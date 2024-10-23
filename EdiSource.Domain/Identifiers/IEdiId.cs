using System.Threading.Channels;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Identifiers;
//
// /// <summary>
// ///     Denotes an IEdi element that has an identifier.
// ///     Used for static abstract interfaces.
// /// </summary>
// // public interface IEdiId : IEdi, IEdiId;
// public interface ISegment<TLoop>
//     : IEdi<TLoop>
//     where TLoop : ILoop
// {
//     /// <summary>
//     ///     Assigns data elements safety one segment to another.
//     ///     Useful for avoiding the issue where two segments share
//     ///     the same reference to a List.
//     ///     The parent and separators will still be shared if not specified.
//     /// </summary>
//     /// <param name="other"></param>
//     /// <param name="separators">Will use segment values if not provided</param>
//     /// <param name="parent">Will use segment values if not provided</param>
//     void Assign(Segment other, Separators? separators = null, ILoop? parent = null);
//
//     /// <summary>
//     ///     Copies data elements safety one segment to a new segment.
//     ///     Useful for avoiding the issue where two segments share
//     ///     the same reference to a List.
//     ///     The parent and separators will still be shared if not specified.
//     ///     <param name="separators">Will use segment values if not provided</param>
//     ///     <param name="parent">Will use segment values if not provided</param>
//     /// </summary>
//     /// <returns></returns>
//     Segment Copy(Separators? separators = null, ILoop? parent = null);
// }