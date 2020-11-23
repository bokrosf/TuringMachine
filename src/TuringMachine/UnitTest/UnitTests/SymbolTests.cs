using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class SymbolTests
    {
        [Fact]
        public void Equals_Reflexive_ReturnsTrue()
        {
            var symbol = new Symbol<int>(5);

            bool areEqual = symbol.Equals(symbol);

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
    }
}
