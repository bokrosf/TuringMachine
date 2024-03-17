using NSubstitute;
using System;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint;

public class StepLimitConstraintTests
{
    private readonly IReadOnlyComputationState computationState;
    
    public StepLimitConstraintTests()
    {
        computationState = Substitute.For<IReadOnlyComputationState>();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(488)]
    public void Constructor_ValidStepLimit_Success(int validStepLimit)
    {
        new StepLimitConstraint(validStepLimit);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-300)]
    public void Constructor_InvalidStepLimit_ThrowsException(int invalidStepLimit)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StepLimitConstraint(invalidStepLimit));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    public void Enforce_LowerThanStepLimit_NotThrowsException(int stepLimit)
    {
        var constraint = new StepLimitConstraint(stepLimit);
        computationState.StepCount.Returns(stepLimit - 1);

        constraint.Enforce(computationState);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void Enforce_StepLimitReached_NotThrowsException(int stepLimit)
    {
        var constraint = new StepLimitConstraint(stepLimit);

        computationState.StepCount.Returns(stepLimit);

        constraint.Enforce(computationState);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void Enforce_StepLimitExceeded_ThrowsException(int stepLimit)
    {
        var constraint = new StepLimitConstraint(stepLimit);
        computationState.StepCount.Returns(stepLimit + 1);

        var violation = constraint.Enforce(computationState) as StepLimitViolation;

        Assert.NotNull(violation);
        Assert.Equal(stepLimit, violation!.StepLimit);
    }
}
