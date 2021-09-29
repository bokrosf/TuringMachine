using System;
using TuringMachine.Machine;
using TuringMachine.Machine.ComputationConstraint;
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
        public void Ensure_LowerThanStepLimit_NotThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));

            for (int i = 0; i < stepLimit - 1; i++)
            {
                computationState.UpdateConfiguration((i, 'G'));
                constraint.Enforce(computationState);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Ensure_StepLimitReached_NotThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));

            for (int i = 0; i < stepLimit; i++)
            {
                computationState.UpdateConfiguration((i, 'G'));
                constraint.Enforce(computationState);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Ensure_FinishedAndStepLimitExceeded_NotThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));

            for (int i = 0; i < stepLimit; i++)
            {
                computationState.UpdateConfiguration((i, 'G'));
                constraint.Enforce(computationState);
            }

            computationState.UpdateConfiguration((stepLimit + 1, 'G'));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Ensure_UnfinishedAndStepLimitExceeded_ThrowsException(int stepLimit)
        {
            var constraint = new StepLimitConstraint<int, char>(stepLimit);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));

            for (int i = 0; i < stepLimit; i++)
            {
                computationState.UpdateConfiguration((i, 'G'));
                constraint.Enforce(computationState);
            }

            computationState.UpdateConfiguration((stepLimit + 1, 'G'));

            Assert.Throws<StepLimitReachedException>(() => constraint.Enforce(computationState));
        }
    }
}
