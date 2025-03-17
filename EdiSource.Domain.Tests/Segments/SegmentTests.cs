using FluentAssertions;
using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using JetBrains.Annotations;

namespace EdiSource.Domain.Tests.Segments;

[TestSubject(typeof(Segment))]
public class SegmentTests
{
    private readonly Separators _separators = new()
    {
        SegmentSeparator = '~',
        DataElementSeparator = '*',
        CompositeElementSeparator = ':'
    };

    [Fact]
    public void Constructor_WithValidSegmentText_ShouldCreateSegmentCorrectly()
    {
        // Arrange & Act
        var segment = new Segment("ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:", _separators);

        // Assert
        segment.Elements.Should().HaveCount(17);
        segment.Elements[0][0].Should().Be("ISA");
        segment.Elements[1][0].Should().Be("00");
    }

    [Fact]
    public void GetCompositeElement_WithValidIndices_ShouldReturnCorrectValue()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _separators);

        // Act
        var value = segment.GetCompositeElement(0);

        // Assert
        value.Should().Be("ST");
    }

    [Fact]
    public void GetCompositeElementOrNull_WithInvalidIndices_ShouldReturnNull()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _separators);

        // Act
        var value = segment.GetCompositeElementOrNull(99);

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void SetDataElement_ShouldUpdateElementValue()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _separators);
        var newValue = "999";

        // Act
        segment.SetDataElement(1, true, newValue);
        var value = segment.GetCompositeElement(1);

        // Assert
        value.Should().Be(newValue);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectlyFormattedString()
    {
        // Arrange
        var originalText = "ST*835*000000001";
        var segment = new Segment(originalText, _separators);

        // Act
        var result = segment.ToString();

        // Assert
        result.Should().Be(originalText + _separators.SegmentSeparator);
    }

    [Fact]
    public void Copy_ShouldCreateDeepCopy()
    {
        // Arrange
        var original = new Segment("ST*835*000000001", _separators);

        // Act
        var copy = original.Copy();
        original.SetDataElement(1, true, "999");

        // Assert
        copy.GetCompositeElement(1).Should().Be("835");
        original.GetCompositeElement(1).Should().Be("999");
    }

    [Theory]
    [InlineData("ST*835*000000001", 3)]
    [InlineData("ISA*00*          *00", 4)]
    public void ElementCount_ShouldBeCorrect(string segmentText, int expectedCount)
    {
        // Arrange & Act
        var segment = new Segment(segmentText, _separators);

        // Assert
        segment.Elements.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void CompositeElementExists_ShouldReturnCorrectResult()
    {
        // Arrange
        var segment = new Segment("ST*835:TEST*000000001", _separators);

        // Act & Assert
        segment.CompositeElementExists(1).Should().BeTrue();
        segment.CompositeElementExists(1, 1).Should().BeTrue();
        segment.CompositeElementExists(99).Should().BeFalse();
    }

    [Fact]
    public void ReadMultipleSegment_ShouldParseCorrectly()
    {
        // Arrange
        var input = "ST*835*1~BPR*DATA*2~SE*3";

        // Act
        var segments = Segment.ReadMultipleSegment(input, _separators).ToList();

        // Assert
        segments.Should().HaveCount(3);
        segments[0].Elements[0][0].Should().Be("ST");
        segments[1].Elements[0][0].Should().Be("BPR");
        segments[2].Elements[0][0].Should().Be("SE");
    }
}