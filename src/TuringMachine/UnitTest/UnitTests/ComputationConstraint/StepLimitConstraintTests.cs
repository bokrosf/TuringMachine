using Moq;
using System;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint
{
    public class StepLimitConstraintTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(488)]
        public void Constructor_ValidStepLimit_Success(int validStepLimit)
        {
            new StepLimitConstraint<int, char>(validStepLimit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-300)]
        public void Constructor_InvalidStepLimit_ThrowsException(int invalidStepLimit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new StepLimitConstraint<int, char>(invalidStepLimit));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void Enforce_LowerThanStepLimit_NotThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);

            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            computationStateMock.Setup(cs => cs.StepCount).Returns(stepLimit - 1);

            constraint.Enforce(computationStateMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Enforce_StepLimitReached_NotThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);

            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            computationStateMock.Setup(cs => cs.StepCount).Returns(stepLimit);

            constraint.Enforce(computationStateMock.Object);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Enforce_StepLimitExceeded_ThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);
            
            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);
            computationStateMock.Setup(cs => cs.StepCount).Returns(stepLimit + 1);

            Assert.Throws<StepLimitExceededException>(() => constraint.Enforce(computationStateMock.Object));
        }
    }
}
