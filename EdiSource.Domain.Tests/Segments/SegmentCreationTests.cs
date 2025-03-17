using EdiSource.Domain.Elements;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Tests.Segments;

[TestSubject(typeof(Segment))]
public class SegmentCreationTests
{
    private readonly Separators _defaultSeparators = new();
    private readonly Separators _customSeparators = new('|', '+', '/');

    [Fact]
    public void Constructor_WithString_ShouldCreateSegmentCorrectly()
    {
        // Arrange & Act
        var segment = new Segment("ST*835*000000001");

        // Assert
        segment.Elements.Should().HaveCount(3);
        segment[0].Should().Be("ST");
        segment[1].Should().Be("835");
        segment[2].Should().Be("000000001");
    }

    [Fact]
    public void Constructor_WithStringAndCustomSeparators_ShouldCreateSegmentCorrectly()
    {
        // Arrange & Act
        var segment = new Segment("ST+835+000000001", _customSeparators);

        // Assert
        segment.Elements.Should().HaveCount(3);
        segment[0].Should().Be("ST");
        segment[1].Should().Be("835");
        segment[2].Should().Be("000000001");
        segment.Separators.DataElementSeparator.Should().Be('+');
    }

    [Fact]
    public void Constructor_WithElementCollection_ShouldCreateSegmentCorrectly()
    {
        // Arrange
        List<Element> elements =
        [
            "ST",
            "835",
            "000000001"
        ];

        // Act
        var segment = new Segment(elements);

        // Assert
        segment.Elements.Should().HaveCount(3);
        segment[0].Should().Be("ST");
        segment[1].Should().Be("835");
        segment[2].Should().Be("000000001");
    }

    [Fact]
    public void Constructor_WithElementCollectionAndCustomSeparators_ShouldCreateSegmentCorrectly()
    {
        // Arrange
        List<Element> elements =
        [
            "ST",
            "835",
            "000000001"
        ];

        // Act
        var segment = new Segment(elements, _customSeparators);

        // Assert
        segment.Elements.Should().HaveCount(3);
        segment.Separators.DataElementSeparator.Should().Be('+');
        segment.Separators.CompositeElementSeparator.Should().Be('/');
        segment.Separators.SegmentSeparator.Should().Be('|');
    }

    [Fact]
    public void Constructor_WithAnotherSegment_ShouldCreateCopy()
    {
        // Arrange
        var originalSegment = new Segment("ST*835*000000001");

        // Act
        var newSegment = new Segment(originalSegment);

        // Assert
        newSegment.Elements.Should().HaveCount(3);
        newSegment[0].Should().Be("ST");
        newSegment[1].Should().Be("835");
        newSegment[2].Should().Be("000000001");

        // Verify it's a copy, not a reference
        originalSegment[1] = "999";
        newSegment[1].Should().Be("835");
    }

    [Fact]
    public void Constructor_WithCompositeElements_ShouldCreateSegmentCorrectly()
    {
        // Arrange & Act
        var segment = new Segment("NM1*IL*1*DOE:JOHN*A", new Separators('~', '*', ':'));

        // Assert
        segment.Elements.Should().HaveCount(5);
        segment.Elements[0][0].Should().Be("NM1");
        segment.Elements[1][0].Should().Be("IL");
        segment.Elements[2][0].Should().Be("1");
        segment.Elements[3][0].Should().Be("DOE");
        segment.Elements[3][1].Should().Be("JOHN"); // Composite element
    }

    [Fact]
    public void Constructor_WithNoElements_ShouldCreateEmptySegment()
    {
        // Arrange & Act
        var segment = new Segment();

        // Assert
        segment.Elements.Should().BeEmpty();
        segment.Separators.Should().BeEquivalentTo(Separators.DefaultSeparators);
    }

    [Fact]
    public void ReadMultipleSegment_ShouldCreateMultipleSegments()
    {
        // Arrange
        const string segmentText = "ST*835*0001~BPR*I*5.75*C~SE*3*0001";

        // Act
        var segments = Segment.ReadMultipleSegment(segmentText).ToList();

        // Assert
        segments.Should().HaveCount(3);
        segments[0][0].Should().Be("ST");
        segments[1][0].Should().Be("BPR");
        segments[2][0].Should().Be("SE");
    }

    [Fact]
    public void ReadMultipleSegment_WithCustomSeparators_ShouldCreateMultipleSegments()
    {
        // Arrange
        const string segmentText = "ST+835+0001|BPR+I+5.75+C|SE+3+0001";

        // Act
        var segments = Segment.ReadMultipleSegment(segmentText, _customSeparators).ToList();

        // Assert
        segments.Should().HaveCount(3);
        segments[0][0].Should().Be("ST");
        segments[1][0].Should().Be("BPR");
        segments[2][0].Should().Be("SE");
    }

    [Fact]
    public void ReadElements_ShouldExtractElementsFromString()
    {
        // Arrange
        const string segmentText = "ST*835*000000001";

        // Act
        var elements = Segment.ReadElements(segmentText).ToList();

        // Assert
        elements.Should().HaveCount(3);
        elements[0][0].Should().Be("ST");
        elements[1][0].Should().Be("835");
        elements[2][0].Should().Be("000000001");
    }

    [Fact]
    public void Copy_ShouldCreateDeepCopy()
    {
        // Arrange
        var original = new Segment("ST*835*000000001");

        // Act
        var copy = original.Copy();

        // Modify original
        original[1] = "999";

        // Assert
        copy[0].Should().Be("ST");
        copy[1].Should().Be("835"); // Should not be affected by the change
        copy[2].Should().Be("000000001");
    }

    [Fact]
    public void Assign_ShouldCopyValuesFromOtherSegment()
    {
        // Arrange
        var source = new Segment("ST*835*000000001");
        var target = new Segment();

        // Act
        target.Assign(source);

        // Assert
        target.Elements.Should().HaveCount(3);
        target[0].Should().Be("ST");
        target[1].Should().Be("835");
        target[2].Should().Be("000000001");
    }

    [Fact]
    public void ToString_ShouldOutputProperlyFormattedSegment()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _defaultSeparators);

        // Act
        var result = segment.ToString();

        // Assert
        result.Should().Be("ST*835*000000001~");
    }

    [Fact]
    public void ToString_WithCustomSeparators_ShouldOutputProperlyFormattedSegment()
    {
        // Arrange
        var segment = new Segment("ST+835+000000001", _customSeparators);

        // Act
        var result = segment.ToString(_customSeparators);

        // Assert
        result.Should().Be("ST+835+000000001|");
    }
}