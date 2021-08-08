using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class StateTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(645)]
        [InlineData(478)]
        [InlineData(21)]
        public void Constructor_ValuePropertySetted_Success(int value)
        {
            var state = new State<int>(value);

            int actualValue = state.Value;

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
            var state = new State<int?>(value);

            int? actualValue = state.Value;

            Assert.Equal(value, actualValue);
        }

        [Fact]
        public void Equals_Reflexive_ReturnsTrue()
        {
            var state = new State<int>(5);

            bool areEqual = state.Equals(state);

            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_InitialReflexive_ReturnsTrue()
        {
            var state = State<int>.Initial;

            bool areEqual = state.Equals(State<int>.Initial);

            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_EndReflexive_ReturnsTrue()
        {
            var state = State<int>.End;

            bool areEqual = state.Equals(State<int>.End);

            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_FailureReflexive_ReturnsTrue()
        {
            var state = State<int>.Failure;

            bool areEqual = state.Equals(State<int>.Failure);

            Assert.True(areEqual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_Symmetric_ReturnsTrue(int sameValue)
        {
            var first = new State<int>(sameValue);
            var second = new State<int>(sameValue);

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
            var first = new State<int?>(sameValue);
            var second = new State<int?>(sameValue);

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
            var first = new State<int>(firstValue);
            var second = new State<int>(secondValue);

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
            var first = new State<int?>(firstValue);
            var second = new State<int?>(secondValue);

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
            var first = new State<int>(firstValue);
            var second = new State<int?>(secondValue);

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
        public void Equals_InitialComparedWithNormalSymmetric_ReturnsFalse(int value)
        {
            var initial = State<int>.Initial;
            var normal = new State<int>(value);

            bool initialEqualsWithNormal = initial.Equals(normal);
            bool normalEqualsWithInitial = normal.Equals(initial);

            Assert.False(initialEqualsWithNormal);
            Assert.False(normalEqualsWithInitial);
        }

        [Theory]
        [InlineData(default(int))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_EndComparedWithNormalSymmetric_ReturnsFalse(int value)
        {
            var end = State<int>.End;
            var normal = new State<int>(value);

            bool endEqualsWithNormal = end.Equals(normal);
            bool normalEqualsWithEnd = normal.Equals(end);

            Assert.False(endEqualsWithNormal);
            Assert.False(normalEqualsWithEnd);
        }

        [Theory]
        [InlineData(default(int))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_FailureComparedWithNormalSymmetric_ReturnsFalse(int value)
        {
            var failure = State<int>.Failure;
            var normal = new State<int>(value);

            bool failureEqualsWithNormal = failure.Equals(normal);
            bool normalEqualsWitFailure = normal.Equals(failure);

            Assert.False(failureEqualsWithNormal);
            Assert.False(normalEqualsWitFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_InitialComparedWithNullableNormalSymmetric_ReturnsFalse(int? value)
        {
            var initial = State<int?>.Initial;
            var normal = new State<int?>(value);

            bool initialEqualsWithNormal = initial.Equals(normal);
            bool normalEqualsWithInitial = normal.Equals(initial);

            Assert.False(initialEqualsWithNormal);
            Assert.False(normalEqualsWithInitial);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_EndComparedWithNullableNormalSymmetric_ReturnsFalse(int? value)
        {
            var end = State<int?>.End;
            var normal = new State<int?>(value);

            bool endEqualsWithNormal = end.Equals(normal);
            bool normalEqualsWithEnd = normal.Equals(end);

            Assert.False(endEqualsWithNormal);
            Assert.False(normalEqualsWithEnd);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(55)]
        [InlineData(32)]
        public void Equals_FailureComparedWithNullableNormalSymmetric_ReturnsFalse(int? value)
        {
            var failure = State<int?>.Failure;
            var normal = new State<int?>(value);

            bool failureEqualsWithNormal = failure.Equals(normal);
            bool normalEqualsWitFailure = normal.Equals(failure);

            Assert.False(failureEqualsWithNormal);
            Assert.False(normalEqualsWitFailure);
        }

        [Fact]
        public void Equals_Transitive_ReturnsTrue()
        {
            const int SameValue = 5;
            var first = new State<int>(SameValue);
            var second = new State<int>(SameValue);
            var third = new State<int>(SameValue);

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
            var state = new State<int>(5);

            bool areEqual = state.Equals(null);

            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_ComparedWithDifferentType_ReturnsFalse()
        {
            var state = new State<int>(5);
            string comparedWith = string.Empty;

            bool areEqual = state.Equals(comparedWith);

            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_InitialComparedWithNull_ReturnsFalse()
        {
            var initial = State<int>.Initial;

            bool areEqual = initial.Equals(null);

            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_EndComparedWithNull_ReturnsFalse()
        {
            var end = State<int>.End;

            bool areEqual = end.Equals(null);

            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_FailureComparedWithNull_ReturnsFalse()
        {
            var failure = State<int>.Failure;

            bool areEqual = failure.Equals(null);

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
            var first = new State<int>(sameValue);
            var second = new State<int>(sameValue);

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
            var first = new State<int?>(sameValue);
            var second = new State<int?>(sameValue);

            int? actual = second.GetHashCode();

            Assert.Equal(first.GetHashCode(), actual);
        }

        [Fact]
        public void GetHashCode_InitialSameHashCode_ReturnsTrue()
        {
            var first = State<int?>.Initial;
            var second = State<int?>.Initial;

            int? actual = second.GetHashCode();

            Assert.Equal(first.GetHashCode(), actual);
        }

        [Fact]
        public void GetHashCode_EndSameHashCode_ReturnsTrue()
        {
            var first = State<int?>.End;
            var second = State<int?>.End;

            int? actual = second.GetHashCode();

            Assert.Equal(first.GetHashCode(), actual);
        }

        [Fact]
        public void GetHashCode_FailureSameHashCode_ReturnsTrue()
        {
            var first = State<int?>.Failure;
            var second = State<int?>.Failure;

            int? actual = second.GetHashCode();

            Assert.Equal(first.GetHashCode(), actual);
        }

        [Fact]
        public void EqualityOperator_BothNull_ReturnsTrue()
        {
            State<int>? left = null;
            State<int>? right = null;

            bool areEqual = left == right;

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualityOperator_OnlyLeftNull_ReturnsFalse()
        {
            State<int>? left = null;
            State<int>? right = new State<int>(5);

            bool areEqual = left == right;

            Assert.False(areEqual);
        }

        [Fact]
        public void EqualityOperator_OnlyRightNull_ReturnsFalse()
        {
            State<int>? left = new State<int>(5);
            State<int>? right = null;

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
        public void EqualityOperator_SameValue_ReturnsTrue(int sameValue)
        {
            State<int> left = new State<int>(sameValue);
            State<int> right = new State<int>(sameValue);

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
            State<int> left = new State<int>(leftValue);
            State<int> right = new State<int>(rightValue);

            bool areEqual = left == right;

            Assert.False(areEqual);
        }
    }
}
