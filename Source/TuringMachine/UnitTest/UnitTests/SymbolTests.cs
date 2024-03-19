using Xunit;

namespace TuringMachine.Tests.UnitTests;

public class SymbolTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(645)]
    [InlineData(478)]
    [InlineData(21)]
    public void Constructor_ValuePropertySetted_Success(int value)
    {
        var symbol = new Symbol<int>(value);

        int actualValue = symbol.Value;

        Assert.Equal(value, actualValue);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(null)]
    [InlineData(478)]
    [InlineData(21)]
    public void Constructor_NullableValuePropertySetted_Success(int? value)
    {
        var symbol = new Symbol<int?>(value);

        int? actualValue = symbol.Value;

        Assert.Equal(value, actualValue);
    }

    [Fact]
    public void Equals_Reflexive_ReturnsTrue()
    {
        var symbol = new Symbol<int>(5);

        bool areEqual = symbol.Equals(symbol);

        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_BlankReflexive_ReturnsTrue()
    {
        var symbol = Symbol<int>.Blank;

        bool areEqual = symbol.Equals(Symbol<int>.Blank);

        Assert.True(areEqual);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(55)]
    [InlineData(32)]
    public void Equals_Symmetric_ReturnsTrue(int sameValue)
    {
        var first = new Symbol<int>(sameValue);
        var second = new Symbol<int>(sameValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithFirst = second.Equals(first);

        Assert.True(firstEqualsWithSecond);
        Assert.True(secondEqualsWithFirst);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(55)]
    [InlineData(32)]
    [InlineData(null)]
    public void Equals_NullableSymmetric_ReturnsTrue(int? sameValue)
    {
        var first = new Symbol<int?>(sameValue);
        var second = new Symbol<int?>(sameValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithFirst = second.Equals(first);

        Assert.True(firstEqualsWithSecond);
        Assert.True(secondEqualsWithFirst);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(55, 42)]
    [InlineData(32, 13)]
    public void Equals_Symmetric_ReturnsFalse(int firstValue, int secondValue)
    {
        var first = new Symbol<int>(firstValue);
        var second = new Symbol<int>(secondValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithFirst = second.Equals(first);

        Assert.False(firstEqualsWithSecond);
        Assert.False(secondEqualsWithFirst);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(55, 42)]
    [InlineData(32, 13)]
    [InlineData(null, 13)]
    [InlineData(32, null)]
    public void Equals_NullableSymmetric_ReturnsFalse(int? firstValue, int? secondValue)
    {
        var first = new Symbol<int?>(firstValue);
        var second = new Symbol<int?>(secondValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithFirst = second.Equals(first);

        Assert.False(firstEqualsWithSecond);
        Assert.False(secondEqualsWithFirst);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(55, 55)]
    [InlineData(32, 13)]
    [InlineData(13, null)]
    [InlineData(32, null)]
    public void Equals_NonNullableComparedWithNullableSymmetric_ReturnsFalse(int firstValue, int? secondValue)
    {
        var first = new Symbol<int>(firstValue);
        var second = new Symbol<int?>(secondValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithFirst = second.Equals(first);

        Assert.False(firstEqualsWithSecond);
        Assert.False(secondEqualsWithFirst);
    }

    [Theory]
    [InlineData(default(int))]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(55)]
    [InlineData(32)]
    public void Equals_BlankComparedWithNormalSymmetric_ReturnsFalse(int value)
    {
        var blank = Symbol<int>.Blank;
        var normal = new Symbol<int>(value);

        bool blankEqualsWithNormal = blank.Equals(normal);
        bool normalEqualsWithBlank = normal.Equals(blank);

        Assert.False(blankEqualsWithNormal);
        Assert.False(normalEqualsWithBlank);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(55)]
    [InlineData(32)]
    public void Equals_BlankComparedWithNullableNormalSymmetric_ReturnsFalse(int? value)
    {
        var blank = Symbol<int?>.Blank;
        var normal = new Symbol<int?>(value);

        bool blankEqualsWithNormal = blank.Equals(normal);
        bool normalEqualsWithBlank = normal.Equals(blank);

        Assert.False(blankEqualsWithNormal);
        Assert.False(normalEqualsWithBlank);
    }

    [Fact]
    public void Equals_Transitive_ReturnsTrue()
    {
        const int SameValue = 5;
        var first = new Symbol<int>(SameValue);
        var second = new Symbol<int>(SameValue);
        var third = new Symbol<int>(SameValue);

        bool firstEqualsWithSecond = first.Equals(second);
        bool secondEqualsWithThird = second.Equals(third);
        bool firstEqualsWithThird = first.Equals(third);

        Assert.True(firstEqualsWithSecond);
        Assert.True(secondEqualsWithThird);
        Assert.True(firstEqualsWithThird);
    }

    [Fact]
    public void Equals_ComparedWithNull_ReturnsFalse()
    {
        var symbol = new Symbol<int>(5);

        bool areEqual = symbol.Equals(null);

        Assert.False(areEqual);
    }

    [Fact]
    public void Equals_ComparedWithDifferentType_ReturnsFalse()
    {
        var symbol = new Symbol<int>(5);
        string comparedWith = string.Empty;

        bool areEqual = symbol.Equals(comparedWith);

        Assert.False(areEqual);
    }

    [Fact]
    public void Equals_BlankComparedWithNull_ReturnsFalse()
    {
        var blank = Symbol<int>.Blank;

        bool areEqual = blank.Equals(null);

        Assert.False(areEqual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(56)]
    [InlineData(54)]
    [InlineData(678)]
    public void GetHashCode_SameHashCode_ReturnsTrue(int sameValue)
    {
        var first = new Symbol<int>(sameValue);
        var second = new Symbol<int>(sameValue);

        int actual = second.GetHashCode();

        Assert.Equal(first.GetHashCode(), actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(56)]
    [InlineData(54)]
    [InlineData(678)]
    public void GetHashCode_NullableSameHashCode_ReturnsTrue(int? sameValue)
    {
        var first = new Symbol<int?>(sameValue);
        var second = new Symbol<int?>(sameValue);

        int? actual = second.GetHashCode();

        Assert.Equal(first.GetHashCode(), actual);
    }

    [Fact]
    public void GetHashCode_BlankSameHashCode_ReturnsTrue()
    {
        var first = Symbol<int?>.Blank;
        var second = Symbol<int?>.Blank;

        int? actual = second.GetHashCode();

        Assert.Equal(first.GetHashCode(), actual);
    }

    [Fact]
    public void EqualityOperator_BothNull_ReturnsTrue()
    {
        Symbol<int>? left = null;
        Symbol<int>? right = null;

        bool areEqual = left == right;

        Assert.True(areEqual);
    }

    [Fact]
    public void EqualityOperator_OnlyLeftNull_ReturnsFalse()
    {
        Symbol<int>? left = null;
        Symbol<int>? right = new Symbol<int>(5);

        bool areEqual = left == right;

        Assert.False(areEqual);
    }

    [Fact]
    public void EqualityOperator_OnlyRightNull_ReturnsFalse()
    {
        Symbol<int>? left = new Symbol<int>(5);
        Symbol<int>? right = null;

        bool areEqual = left == right;

        Assert.False(areEqual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(56)]
    [InlineData(54)]
    [InlineData(678)]
    public void EqualityOperator_SameValue_ReturnsFalse(int sameValue)
    {
        Symbol<int> left = new Symbol<int>(sameValue);
        Symbol<int> right = new Symbol<int>(sameValue);

        bool areEqual = left == right;

        Assert.True(areEqual);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 89)]
    [InlineData(56, 3)]
    [InlineData(54, 93)]
    [InlineData(678, 35)]
    public void EqualityOperator_DifferentValue_ReturnsFalse(int leftValue, int rightValue)
    {
        Symbol<int> left = new Symbol<int>(leftValue);
        Symbol<int> right = new Symbol<int>(rightValue);

        bool areEqual = left == right;

        Assert.False(areEqual);
    }
}
