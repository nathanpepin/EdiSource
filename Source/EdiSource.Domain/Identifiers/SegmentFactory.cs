namespace EdiSource.Domain.Identifiers;

/// <summary>
///     Used for creating segments dynamically with enhanced error reporting and near-miss detection
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TLoop"></typeparam>
public static class SegmentLoopFactory<T, TLoop>
    where T : Segment, IEdi<TLoop>, ISegmentIdentifier<T>, new()
    where TLoop : class, ILoop
{
    private static readonly ConcurrentDictionary<Type, List<SegmentInfo>> LoopSegmentCache = new();

    /// <summary>
    ///     Creates a segment if it matches the criteria, with enhanced error messages and near-miss detection
    /// </summary>
    public static async ValueTask<T> CreateAsync(ChannelReader<Segment> segmentReader, TLoop? parent = null)
    {
        await segmentReader.WaitToReadAsync();

        if (!await ISegmentIdentifier<T>.MatchesAsync(segmentReader))
        {
            var receivedSegment = await segmentReader.ReadAsync();

            // Collect near misses for enhanced error reporting
            var nearMisses = CollectNearMisses(receivedSegment, parent);

            throw new EdiParsingException(BuildEnhancedErrorMessage(receivedSegment, parent, nearMisses));
        }

        var segment = await segmentReader.ReadAsync();
        var t = new T { Elements = segment.Elements, Separators = segment.Separators };

        if (t is IEdi<TLoop> e)
            e.Parent = parent;

        return t;
    }

    /// <summary>
    /// Collects information about segments that almost matched by testing known segment types
    /// </summary>
    private static List<NearMiss> CollectNearMisses(Segment receivedSegment, TLoop? parent)
    {
        var nearMisses = new List<NearMiss>();
        var visitedTypes = new HashSet<Type>();

        // Start with current loop and work upward through the hierarchy
        ILoop? currentLoop = parent;

        while (currentLoop != null && visitedTypes.Count < 20) // Limit search to prevent performance issues
        {
            var loopType = currentLoop.GetType();

            if (!visitedTypes.Add(loopType))
                break;

            // Get segment types that might be relevant for this loop
            var segmentInfos = GetRelevantSegmentTypes(loopType, receivedSegment);

            foreach (var segmentInfo in segmentInfos)
            {
                var matchLevel = CalculateMatchLevel(receivedSegment, segmentInfo);

                if (matchLevel > 0) // Only include actual partial matches
                {
                    nearMisses.Add(new NearMiss(
                        segmentInfo.TypeName,
                        segmentInfo.EdiId,
                        loopType.Name,
                        matchLevel,
                        GetElementsFromSegment(receivedSegment),
                        segmentInfo.EdiId.Split('*'),
                        GetMatchDescription(receivedSegment, segmentInfo, matchLevel)));
                }
            }

            // Move to parent loop
            currentLoop = GetParentLoop(currentLoop);
        }

        return nearMisses.OrderByDescending(nm => nm.MatchLevel).Take(10).ToList();
    }

    private static List<SegmentInfo> GetRelevantSegmentTypes(Type loopType, Segment receivedSegment)
    {
        return LoopSegmentCache.GetOrAdd(loopType, _ =>
        {
            var segmentInfos = new List<SegmentInfo>();
            var receivedTag = receivedSegment.GetCompositeElementOrNull(0, 0);

            // Use a simple approach: look for types in the same namespace that likely belong to this loop
            var assembly = loopType.Assembly;
            var loopNamespace = loopType.Namespace;

            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || !type.Namespace?.StartsWith(loopNamespace ?? "") == true)
                    continue;

                // Look for types that implement ISegmentIdentifier
                if (!type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISegmentIdentifier<>))) continue;
                try
                {
                    // Try to get the EdiId static property
                    var ediIdProperty = type.GetProperty("EdiId", BindingFlags.Public | BindingFlags.Static);
                    if (ediIdProperty?.GetValue(null) is EdiId ediId)
                    {
                        var ediIdString = ediId.ToString();
                        var segmentTag = ediIdString.Split('*')[0];

                        // Include all segments, but prioritize those with matching tags
                        segmentInfos.Add(new SegmentInfo(
                            type.Name,
                            ediIdString,
                            segmentTag,
                            segmentTag == receivedTag));
                    }
                }
                catch
                {
                    // Skip segments we can't analyze
                }
            }

            // Return tag matches first, then others
            return segmentInfos.OrderByDescending(si => si.IsTagMatch).ToList();
        });
    }

    private static int CalculateMatchLevel(Segment receivedSegment, SegmentInfo segmentInfo)
    {
        var receivedElements = GetElementsFromSegment(receivedSegment);
        var expectedElements = segmentInfo.EdiId.Split('*');

        var matchLevel = 0;
        var maxCheck = Math.Min(receivedElements.Length, expectedElements.Length);

        for (var i = 0; i < maxCheck; i++)
        {
            if (i == 0) // Segment tag
            {
                if (receivedElements[i] == expectedElements[i])
                    matchLevel++;
                else
                    break; // If segment tag doesn't match, no point continuing
            }
            else // Element comparison with multi-value support
            {
                var expectedValues = expectedElements[i].Split('|', StringSplitOptions.RemoveEmptyEntries);
                if (expectedValues.Contains(receivedElements[i]) || string.IsNullOrEmpty(expectedElements[i]))
                    matchLevel++;
                else
                    break; // Stop at first mismatch for meaningful match levels
            }
        }

        return matchLevel;
    }

    private static string[] GetElementsFromSegment(Segment segment)
    {
        var elements = new List<string>();

        // Get first 4 elements for comparison
        for (var i = 0; i < 4; i++)
        {
            var element = segment.GetCompositeElementOrNull(i, 0);
            elements.Add(element ?? ""); // Keep position for accurate comparison
        }

        return elements.ToArray();
    }

    private static string GetMatchDescription(Segment receivedSegment, SegmentInfo segmentInfo, int matchLevel)
    {
        var received = GetElementsFromSegment(receivedSegment);

        return matchLevel switch
        {
            1 => $"Segment tag matches ({received[0]}) but elements differ",
            2 => $"Tag and first element match ({received[0]}*{received[1]}) but later elements differ",
            >= 3 => $"Most elements match, minor differences in element {matchLevel + 1}+",
            _ => "No significant match"
        };
    }

    private static ILoop? GetParentLoop(ILoop loop)
    {
        // Use reflection to find Parent property
        var parentProperty = loop.GetType().GetProperty("Parent");
        return parentProperty?.GetValue(loop) as ILoop;
    }

    private static string BuildEnhancedErrorMessage(Segment receivedSegment, TLoop? parent, List<NearMiss> nearMisses)
    {
        var sb = new StringBuilder();

        // Header
        sb.AppendLine("🚫 EDI PARSING ERROR: Segment Mismatch");
        sb.AppendLine(new string('═', 50));
        sb.AppendLine();

        // Core error info
        sb.AppendLine($"Expected: {T.EdiId} ({typeof(T).Name})");
        sb.AppendLine($"Received: {receivedSegment}");
        sb.AppendLine();

        // Context
        sb.AppendLine("📍 PARSING CONTEXT:");
        sb.AppendLine($"   Current Loop: {typeof(TLoop).Name}");
        sb.AppendLine($"   Expected Segment: {typeof(T).Name}");
        sb.AppendLine($"   Expected Pattern: {T.EdiId}");
        sb.AppendLine();

        // Received segment analysis
        var receivedTag = receivedSegment.GetCompositeElementOrNull(0, 0) ?? "null";
        var element1 = receivedSegment.GetCompositeElementOrNull(1, 0);
        var element2 = receivedSegment.GetCompositeElementOrNull(2, 0);

        sb.AppendLine("📋 RECEIVED SEGMENT:");
        sb.AppendLine($"   Tag: {receivedTag}");
        if (element1 != null) sb.AppendLine($"   Element 1: {element1}");
        if (element2 != null) sb.AppendLine($"   Element 2: {element2}");
        sb.AppendLine();

        // Near misses section
        if (nearMisses.Count != 0)
        {
            sb.AppendLine("🎯 CLOSE MATCHES FOUND:");
            sb.AppendLine();

            var topMatches = nearMisses.Take(5);
            foreach (var nearMiss in topMatches)
            {
                sb.AppendLine($"   • {nearMiss.SegmentTypeName} (Match Level {nearMiss.MatchLevel})");
                sb.AppendLine($"     Expected: {nearMiss.ExpectedEdiId}");
                sb.AppendLine($"     Context: {nearMiss.ParentLoopType}");
                sb.AppendLine($"     Issue: {nearMiss.MatchDescription}");
                sb.AppendLine();
            }

            // Provide targeted advice based on best match
            var bestMatch = nearMisses.First();
            switch (bestMatch.MatchLevel)
            {
                case >= 2:
                {
                    sb.AppendLine("💡 SUGGESTED FIX:");
                    sb.AppendLine($"   Your {bestMatch.SegmentTypeName} segment is very close!");
                    sb.AppendLine($"   Consider updating its [SegmentGenerator] attribute to include:");
                    if (element1 != null)
                    {
                        sb.AppendLine($"   [SegmentGenerator<{bestMatch.ParentLoopType}>(\"{receivedTag}\", \"{element1}\")]");
                        sb.AppendLine($"   or add \"{element1}\" to existing entity codes using {{S}} separator");
                    }

                    sb.AppendLine();
                    break;
                }
                case 1:
                    sb.AppendLine("💡 SUGGESTED FIX:");
                    sb.AppendLine($"   You have a {receivedTag} segment but it doesn't handle entity code '{element1}'");
                    sb.AppendLine($"   Add '{element1}' to the existing segment's entity codes:");
                    sb.AppendLine($"   [SegmentGenerator<{bestMatch.ParentLoopType}>(\"{receivedTag}\", $\"EXISTING{{S}}{element1}\")]");
                    sb.AppendLine();
                    break;
            }
        }
        else
        {
            sb.AppendLine("❓ NO CLOSE MATCHES FOUND");
            sb.AppendLine("   This suggests the segment may belong to a different loop or be completely missing");
            sb.AppendLine();
        }

        // Generic troubleshooting
        sb.AppendLine("🔧 COMMON SOLUTIONS:");
        sb.AppendLine("   1. Add missing entity identifier to existing segment");
        sb.AppendLine("   2. Create new segment implementation for this entity code");
        sb.AppendLine("   3. Check if segment belongs to different loop level");
        sb.AppendLine("   4. Make optional segments nullable in loop definition");
        sb.AppendLine("   5. Verify LS/LE loop boundaries have correct identifiers");

        return sb.ToString();
    }
}

/// <summary>
///     Metadata about a segment type used during near-miss analysis when a segment fails to match.
/// </summary>
/// <param name="TypeName">The short name of the segment type.</param>
/// <param name="EdiId">The expected EDI identifier pattern (e.g. "NM1*IL").</param>
/// <param name="SegmentTag">The segment tag portion of the EDI identifier (e.g. "NM1").</param>
/// <param name="IsTagMatch">Whether this segment's tag matches the received segment's tag.</param>
public sealed record SegmentInfo(string TypeName, string EdiId, string SegmentTag, bool IsTagMatch);

/// <summary>
///     Represents a segment that partially matched the received segment during parsing,
///     used to provide diagnostic near-miss information in error messages.
/// </summary>
/// <param name="SegmentTypeName">The short name of the near-miss segment type.</param>
/// <param name="ExpectedEdiId">The EDI identifier pattern that was expected.</param>
/// <param name="ParentLoopType">The parent loop type where this segment was found.</param>
/// <param name="MatchLevel">How many elements matched (higher = closer match).</param>
/// <param name="ReceivedElements">The first few elements from the received segment.</param>
/// <param name="ExpectedElements">The expected elements from the near-miss segment's EDI identifier.</param>
/// <param name="MatchDescription">A human-readable description of how the match differed.</param>
public sealed record NearMiss(
    string SegmentTypeName,
    string ExpectedEdiId,
    string ParentLoopType,
    int MatchLevel,
    string[] ReceivedElements,
    string[] ExpectedElements,
    string MatchDescription);

/// <summary>
///     Exception thrown when an EDI segment does not match the expected pattern during parsing.
///     Includes contextual information about the expected and received segments for diagnostics.
/// </summary>
public sealed class EdiParsingException : ArgumentException
{
    /// <summary>The segment pattern that was expected.</summary>
    public string? ExpectedSegment { get; }

    /// <summary>The segment that was actually received.</summary>
    public string? ReceivedSegment { get; }

    /// <summary>The parent loop where the mismatch occurred.</summary>
    public string? ParentLoop { get; }

    public EdiParsingException(string message) : base(message)
    {
    }

    public EdiParsingException(string message, string? expectedSegment, string? receivedSegment, string? parentLoop)
        : base(message)
    {
        ExpectedSegment = expectedSegment;
        ReceivedSegment = receivedSegment;
        ParentLoop = parentLoop;
    }

    public EdiParsingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}