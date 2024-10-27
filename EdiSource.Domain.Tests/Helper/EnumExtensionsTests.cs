using EdiSource.Domain.Helper;

namespace EdiSource.Domain.Tests.Helper;

public sealed class EnumExtensionsTests
{
    [Fact]
    public void EnumToStringArray_ReturnsEmpty_WhenEmptyEnum()
    {
        //Arrange
        //Act
        var result = EnumExtensions.EnumToStringArray<EmptyEnum>();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void EnumToStringArray_ReturnsEmpty_WhenEmptyEnumAndKeepUnderscore()
    {
        //Arrange
        //Act
        var result = EnumExtensions.EnumToStringArray<EmptyEnum>(false);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void EnumToStringArray_ReturnsCorrectValues_WithValues()
    {
        //Arrange
        string[] expected = ["A", "B", "C", "D"];

        //Act
        var result = EnumExtensions.EnumToStringArray<TestEnum>();

        //Assert
        expected.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void EnumToStringArray_ReturnsCorrectValues_WithValuesAndKeepUnderscore()
    {
        //Arrange
        string[] expected = ["A", "B", "C", "D"];

        //Act
        var result = EnumExtensions.EnumToStringArray<TestEnum>(false);

        //Assert
        expected.Should().BeEquivalentTo(result);
    }


    [Fact]
    public void EnumToStringArray_ReturnsCorrectValues_WithValuesAndUnderscore()
    {
        //Arrange
        string[] expected = ["1", "2", "3", "4"];

        //Act
        var result = EnumExtensions.EnumToStringArray<TestEnumWithUnderscore>();

        //Assert
        expected.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void EnumToStringArray_ReturnsUnderscoreValues_WithValuesAndUnderscore()
    {
        //Arrange
        string[] expected = ["_1", "_2", "_3", "_4"];

        //Act
        var result = EnumExtensions.EnumToStringArray<TestEnumWithUnderscore>(false);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    private enum EmptyEnum;

    private enum TestEnum
    {
        A,
        B,
        C,
        D
    }

    private enum TestEnumWithUnderscore
    {
        _1,
        _2,
        _3,
        _4
    }
}