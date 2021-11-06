using Moq;
using System;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint
{
    public class TimeLimitConstraintTests
    {
        [Fact]
        public void Constructor_TimeLimitLessThanZero_ThrowsException()
        {
            TimeSpan lessThanZero = TimeSpan.Zero.Subtract(TimeSpan.FromTicks(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeLimitConstraint<int, char>(lessThanZero));
        }

        [Fact]
        public void Constructor_TimeLimitIsZero_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeLimitConstraint<int, char>(TimeSpan.Zero));
        }

        [Fact]
        public void Constructor_TimeLimitGreaterThanZero_Success()
        {
            TimeSpan greaterThanZero = TimeSpan.Zero.Add(TimeSpan.FromTicks(1));
            new TimeLimitConstraint<int, char>(greaterThanZero);
        }

        [Fact]
        public void Enforce_TimeLimitUnreached_NotThrowsException()
        {
            TimeSpan duration = TimeSpan.FromSeconds(3);
            TimeSpan timeLimit = duration.Add(TimeSpan.FromSeconds(2));
            var constraint = new TimeLimitConstraint<int, char>(timeLimit);

            var mockComputationState = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            mockComputationState.Setup(ms => ms.Duration).Returns(duration);

            constraint.Enforce(mockComputationState.Object);
        }

        [Fact]
        public void Enforce_TimeLimitReached_NotThrowsException()
        {
            TimeSpan timeLimit = TimeSpan.FromSeconds(2);
            var constraint = new TimeLimitConstraint<int, char>(timeLimit);

            var mockComputationState = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            mockComputationState.Setup(ms => ms.Duration).Returns(timeLimit);

            constraint.Enforce(mockComputationState.Object);
        }

        [Fact]
        public void Enforce_TimeLimitExceeded_ThrowsException()
        {
            TimeSpan timeLimit = TimeSpan.FromSeconds(3);
            TimeSpan duration = timeLimit.Add(TimeSpan.FromSeconds(2));
            var constraint = new TimeLimitConstraint<int, char>(timeLimit);

            var mockComputationState = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            mockComputationState.Setup(ms => ms.Duration).Returns(duration);

            Assert.Throws<TimeLimitExceededException>(() => constraint.Enforce(mockComputationState.Object));
        }
    }
}
