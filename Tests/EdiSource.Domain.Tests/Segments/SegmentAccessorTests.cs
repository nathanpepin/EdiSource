namespace EdiSource.Domain.Tests.Segments;

[TestSubject(typeof(Segment))]
public class SegmentAccessorsTests
{
    private readonly Separators _separators = new('~', '*', ':');

    [Fact]
    public void GetElement_WithValidIndex_ShouldReturnElement()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var element = segment.GetElement(1);

        // Assert
        element.Should().BeOfType<Element>();
        element[0].Should().Be("835");
    }

    [Fact]
    public void GetElementOrNull_WithValidIndex_ShouldReturnElement()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var element = segment.GetElementOrNull(1);

        // Assert
        element.Should().NotBeNull();
        element![0].Should().Be("835");
    }

    [Fact]
    public void GetElementOrNull_WithInvalidIndex_ShouldReturnNull()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var element = segment.GetElementOrNull(99);

        // Assert
        element.Should().BeNull();
    }

    [Fact]
    public void GetCompositeElement_WithValidIndices_ShouldReturnValue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var value = segment.GetCompositeElement(3, 1);

        // Assert
        value.Should().Be("JOHN");
    }

    [Fact]
    public void GetCompositeElementOrNull_WithInvalidIndices_ShouldReturnNull()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var value = segment.GetCompositeElementOrNull(99);

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void Indexer_WithSingleIndex_ShouldGetAndSetValue()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var value = segment[1];
        segment[1] = "999";

        // Assert
        value.Should().Be("835");
        segment[1].Should().Be("999");
    }

    [Fact]
    public void Indexer_WithDoubleIndex_ShouldGetAndSetValue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var value = segment[3, 1];
        segment[3, 1] = "SMITH";

        // Assert
        value.Should().Be("JOHN");
        segment[3, 1].Should().Be("SMITH");
    }

    [Fact]
    public void SetDataElement_ShouldSetValues()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var result = segment.SetDataElement(1, true, "999", "888");

        // Assert
        result.Should().BeTrue();
        segment.GetElement(1).Should().HaveCount(2);
        segment.GetElement(1)[0].Should().Be("999");
        segment.GetElement(1)[1].Should().Be("888");
    }

    [Fact]
    public void SetDataElement_WithCreate_ShouldCreateNewElement()
    {
        // Arrange
        var segment = new Segment("ST*835");

        // Act
        var result = segment.SetDataElement(2, true, "000000001");

        // Assert
        result.Should().BeTrue();
        segment.GetElement(2)[0].Should().Be("000000001");
    }

    [Fact]
    public void SetDataElement_WithoutCreateAndInvalidIndex_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("ST*835");

        // Act
        var result = segment.SetDataElement(99, false, "Value");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SetCompositeElement_ShouldSetValue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var result = segment.SetCompositeElement("SMITH", 3);

        // Assert
        result.Should().BeTrue();
        segment.GetCompositeElement(3).Should().Be("SMITH");
    }

    [Fact]
    public void SetCompositeElement_WithCreate_ShouldCreateElements()
    {
        // Arrange
        var segment = new Segment("ST*835");

        // Act
        var result = segment.SetCompositeElement("SMITH", 5, 1);

        // Assert
        result.Should().BeTrue();
        segment.Elements.Should().HaveCount(6); // 0-5
        segment.GetCompositeElement(5, 1).Should().Be("SMITH");
        segment.GetCompositeElement(5).Should().BeEmpty(); // First composite element should be empty
    }

    [Fact]
    public void ElementExists_WithValidIndex_ShouldReturnTrue()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var result = segment.ElementExists(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ElementExists_WithInvalidIndex_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");

        // Act
        var result = segment.ElementExists(99);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CompositeElementExists_WithValidIndices_ShouldReturnTrue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var result = segment.CompositeElementExists(3, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CompositeElementExists_WithInvalidIndices_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var result = segment.CompositeElementExists(3, 99);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CompositeElementNotNullOrEmpty_WithNonEmptyValue_ShouldReturnTrue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var result = segment.CompositeElementNotNullOrEmpty(3, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CompositeElementNotNullOrEmpty_WithEmptyValue_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:", _separators);

        // Act
        var result = segment.CompositeElementNotNullOrEmpty(3, 1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetCompositeElementOrNull_WithExistingCompositeElement_ShouldReturnValue()
    {
        // Arrange
        var segment = new Segment("NM1*IL*1*DOE:JOHN", _separators);

        // Act
        var value = segment.GetCompositeElementOrNull(3, 1);

        // Assert
        value.Should().Be("JOHN");
    }

    [Fact]
    public void Segments_SettersAndGetters_ShouldInitializeWithDefaultSeparators()
    {
        // Arrange
        var segment = new Segment();

        // Act & Assert
        segment.Separators.Should().NotBeNull();
        segment.Separators.SegmentSeparator.Should().Be('~');
        segment.Separators.DataElementSeparator.Should().Be('*');
        segment.Separators.CompositeElementSeparator.Should().Be(':');
    }

    [Fact]
    public void Segments_SettersAndGetters_ShouldAllowCustomSeparators()
    {
        // Arrange
        var customSeparators = new Separators('|', '+', '/');
        var segment = new Segment();

        // Act
        segment.Separators = customSeparators;

        // Assert
        segment.Separators.SegmentSeparator.Should().Be('|');
        segment.Separators.DataElementSeparator.Should().Be('+');
        segment.Separators.CompositeElementSeparator.Should().Be('/');
    }
}