using EdiSource.Domain.Separator;
using FluentAssertions;
using JetBrains.Annotations;

namespace EdiSource.Domain.Tests.Separator;

[TestSubject(typeof(Separators))]
public class SeparatorsTests
{
    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetCorrectDefaults()
    {
        // Arrange & Act
        var separators = new Separators();

        // Assert
        separators.SegmentSeparator.Should().Be('~');
        separators.DataElementSeparator.Should().Be('*');
        separators.CompositeElementSeparator.Should().Be(':');
    }

    [Fact]
    public void Constructor_WithCustomValues_ShouldSetCorrectValues()
    {
        // Arrange & Act
        var separators = new Separators('|', '+', '/');

        // Assert
        separators.SegmentSeparator.Should().Be('|');
        separators.DataElementSeparator.Should().Be('+');
        separators.CompositeElementSeparator.Should().Be('/');
    }

    [Fact]
    public async Task CreateFromISA_WithValidISASegment_ShouldExtractSeparators()
    {
        // Arrange
        var isaText =
            "ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~";
        using var reader = new StringReader(isaText);
        using var streamReader = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(isaText)));

        // Act
        var separators = await Separators.CreateFromISA(streamReader);

        // Assert
        separators.DataElementSeparator.Should().Be('*');
        separators.CompositeElementSeparator.Should().Be(':');
        separators.SegmentSeparator.Should().Be('~');
    }

    [Fact]
    public void CreateFromISA_WithValidISAText_ShouldExtractSeparators()
    {
        // Arrange
        var isaText =
            "ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~";

        // Act
        var separators = Separators.CreateFromISA(isaText);

        // Assert
        separators.DataElementSeparator.Should().Be('*');
        separators.CompositeElementSeparator.Should().Be(':');
        separators.SegmentSeparator.Should().Be('~');
    }

    [Fact]
    public void DefaultSeparators_ShouldHaveCorrectValues()
    {
        // Arrange & Act
        var defaultSeparators = Separators.DefaultSeparators;

        // Assert
        defaultSeparators.SegmentSeparator.Should().Be('~');
        defaultSeparators.DataElementSeparator.Should().Be('*');
        defaultSeparators.CompositeElementSeparator.Should().Be(':');
    }
}