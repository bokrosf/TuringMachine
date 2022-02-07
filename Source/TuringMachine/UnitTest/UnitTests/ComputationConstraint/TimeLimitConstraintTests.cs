using Moq;
using System;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint;

public class TimeLimitConstraintTests
{
    [Fact]
    public void Constructor_TimeLimitLessThanZero_ThrowsException()
    {
        TimeSpan lessThanZero = TimeSpan.Zero.Subtract(TimeSpan.FromTicks(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TimeLimitConstraint(lessThanZero));
    }

    [Fact]
    public void Constructor_TimeLimitIsZero_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TimeLimitConstraint(TimeSpan.Zero));
    }

    [Fact]
    public void Constructor_TimeLimitGreaterThanZero_Success()
    {
        TimeSpan greaterThanZero = TimeSpan.Zero.Add(TimeSpan.FromTicks(1));
        new TimeLimitConstraint(greaterThanZero);
    }

    [Fact]
    public void Enforce_TimeLimitUnreached_NotThrowsException()
    {
        TimeSpan duration = TimeSpan.FromSeconds(3);
        TimeSpan timeLimit = duration.Add(TimeSpan.FromSeconds(2));
        var constraint = new TimeLimitConstraint(timeLimit);

        var mockComputationState = new Mock<IReadOnlyComputationState>(MockBehavior.Strict);
        mockComputationState.Setup(ms => ms.Duration).Returns(duration);

        constraint.Enforce(mockComputationState.Object);
    }

    [Fact]
    public void Enforce_TimeLimitReached_NotThrowsException()
    {
        TimeSpan timeLimit = TimeSpan.FromSeconds(2);
        var constraint = new TimeLimitConstraint(timeLimit);

        var mockComputationState = new Mock<IReadOnlyComputationState>(MockBehavior.Strict);
        mockComputationState.Setup(ms => ms.Duration).Returns(timeLimit);

        constraint.Enforce(mockComputationState.Object);
    }

    [Fact]
    public void Enforce_TimeLimitExceeded_ThrowsException()
    {
        TimeSpan timeLimit = TimeSpan.FromSeconds(3);
        TimeSpan duration = timeLimit.Add(TimeSpan.FromSeconds(2));
        var constraint = new TimeLimitConstraint(timeLimit);
        var mockComputationState = new Mock<IReadOnlyComputationState>(MockBehavior.Strict);
        mockComputationState.Setup(ms => ms.Duration).Returns(duration);

        var violation = constraint.Enforce(mockComputationState.Object) as TimeLimitViolation;

        Assert.NotNull(violation);
        Assert.Equal(timeLimit, violation!.TimeLimit);
        Assert.Equal(mockComputationState.Object.Duration, violation!.Duration);
    }
}
