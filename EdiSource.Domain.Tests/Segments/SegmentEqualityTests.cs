using EdiSource.Domain.Elements;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Tests.Segments;

[TestSubject(typeof(Segment))]
public class SegmentEqualityTests
{
    private readonly Separators _separators = new('~', '*', ':');

    [Fact]
    public void Equals_WithIdenticalSegments_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("ST*835*000000001");

        // Act & Assert
        segment1.Equals(segment2).Should().BeTrue();
        (segment1 == segment2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentSegments_ShouldReturnFalse()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("ST*837*000000001");

        // Act & Assert
        segment1.Equals(segment2).Should().BeFalse();
        (segment1 == segment2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentElementCounts_ShouldReturnFalse()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("ST*835");

        // Act & Assert
        segment1.Equals(segment2).Should().BeFalse();
        (segment1 == segment2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithSameElementCountsButDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("BPR*835*000000001");

        // Act & Assert
        segment1.Equals(segment2).Should().BeFalse();
        (segment1 == segment2).Should().BeFalse();
    }

    [Fact]
    public void NotEquals_WithDifferentSegments_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("ST*837*000000001");

        // Act & Assert
        (segment1 != segment2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithNonSegmentObject_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");
        var nonSegment = new object();

        // Act & Assert
        segment.Equals(nonSegment).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act & Assert
        segment.Equals(null!).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithIdenticalCompositeElements_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment("NM1*IL*1*DOE:JOHN", _separators);
        var segment2 = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act & Assert
        segment1.Equals(segment2).Should().BeTrue();
        (segment1 == segment2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentCompositeElements_ShouldReturnFalse()
    {
        // Arrange
        var segment1 = new Segment("NM1*IL*1*DOE:JOHN", _separators);
        var segment2 = new Segment("NM1*IL*1*DOE:JANE", _separators);

        // Act & Assert
        segment1.Equals(segment2).Should().BeFalse();
        (segment1 == segment2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithIdenticalSegments_ShouldReturnSameHashCode()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment("ST*835*000000001");

        // Act
        var hashCode1 = segment1.GetHashCode();
        var hashCode2 = segment2.GetHashCode();

        // Assert
        // Note: Hash codes are not guaranteed to be equal for equal objects,
        // but for our testing purposes we should expect that our implementation
        // produces the same hash code for identical segments
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void CopyConstructor_ShouldCreateEqualSegment()
    {
        // Arrange
        var original = new Segment("ST*835*000000001");

        // Act
        var copy = new Segment(original);

        // Assert
        copy.Should().Be(original);
        (copy == original).Should().BeTrue();
    }

    [Fact]
    public void Copy_ShouldCreateEqualSegment()
    {
        // Arrange
        var original = new Segment("ST*835*000000001");

        // Act
        var copy = original.Copy();

        // Assert
        copy.Should().Be(original);
        (copy == original).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentSeparatorsButSameElements_ShouldReturnTrue()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001", new Separators('~', '*', ':'));
        var segment2 = new Segment("ST+835+000000001", new Separators('|', '+', '/'));

        // Act & Assert
        // Equality should be based on the elements' values, not the separators
        segment1.Equals(segment2).Should().BeTrue();
        (segment1 == segment2).Should().BeTrue();
    }

    [Fact]
    public void Element_ModificationAfterCopy_ShouldNotAffectEquality()
    {
        // Arrange
        var segment1 = new Segment("ST*835*000000001");
        var segment2 = new Segment(segment1);

        // Act - modify segment1
        segment1[1] = "837";

        // Assert - segment2 should still have the original value
        segment2[1].Should().Be("835");
        segment1.Equals(segment2).Should().BeFalse();
        (segment1 == segment2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithReorderedElementsList_ShouldBehaveProperly()
    {
        // Arrange
        List<Element> elements1 =
        [
            "ST",
            "835",
            "000000001"
        ];

        List<Element> elements2 =
        [
            "ST",
            "835",
            "000000001"
        ];

        var segment1 = new Segment(elements1);
        var segment2 = new Segment(elements2);

        // Act & Assert
        segment1.Equals(segment2).Should().BeTrue();
        (segment1 == segment2).Should().BeTrue();
    }
}