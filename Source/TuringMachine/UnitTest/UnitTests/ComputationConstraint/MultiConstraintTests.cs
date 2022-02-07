using Moq;
using System;
using System.Linq;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint;

public class MultiConstraintTests
{
    [Fact]
    public void Constructor_EmptyConstraintList_ThrowsException()
    {
        var contraints = Enumerable.Empty<IComputationConstraint<IReadOnlyComputationState>>();
        Assert.Throws<ArgumentException>(() => new MultiConstraint(contraints));
    }

    [Fact]
    public void Enforce_SingleValidConstraint_NotThrowsException()
    {
        var mockComputationState = new Mock<IReadOnlyComputationState>();
        var mockSingleConstraint = new Mock<IComputationConstraint<IReadOnlyComputationState>>();
        var constraints = new IComputationConstraint<IReadOnlyComputationState>[] { mockSingleConstraint.Object };
        var multiConstraint = new MultiConstraint(constraints);

        multiConstraint.Enforce(mockComputationState.Object);
    }

    [Fact]
    public void Enforce_AllConstraintsValid_NotThrowsException()
    {
        var mockComputationState = new Mock<IReadOnlyComputationState>();
        var constraints = Enumerable.Range(0, 5).Select(i => new Mock<IComputationConstraint<IReadOnlyComputationState>>().Object);
        var multiConstraint = new MultiConstraint(constraints);

        multiConstraint.Enforce(mockComputationState.Object);
    }

    [Fact]
    public void Enforce_SingleInvalidConstraint_ThrowsException()
    {
        var mockComputationState = new Mock<IReadOnlyComputationState>();
        var constraints = new IComputationConstraint<IReadOnlyComputationState>[] { CreateInvalidConstraintMock<int, char>() };
        var multiConstraint = new MultiConstraint(constraints);

        var violation = multiConstraint.Enforce(mockComputationState.Object) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(1, violation!.Violations.Count);
    }

    [Fact]
    public void Enforce_AllConstraintsInvalid_ThrowsException()
    {
        var mockComputationState = new Mock<IReadOnlyComputationState>();
        var constraints = Enumerable.Range(0, 5).Select(i => CreateInvalidConstraintMock<int, char>());
        var multiConstraint = new MultiConstraint(constraints);

        var violation = multiConstraint.Enforce(mockComputationState.Object) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(constraints.Count(), violation!.Violations.Count);
    }

    [Fact]
    public void Enforce_MixedValidityConstraints_ThrowsException()
    {
        var mockComputationState = new Mock<IReadOnlyComputationState>();
        var validConstraints = Enumerable.Range(0, 5).Select(i => new Mock<IComputationConstraint<IReadOnlyComputationState>>().Object);
        var invalidConstraints = Enumerable.Range(0, 5).Select(i => CreateInvalidConstraintMock<int, char>());
        var multiConstraint = new MultiConstraint(validConstraints.Concat(invalidConstraints));

        var violation = multiConstraint.Enforce(mockComputationState.Object) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(invalidConstraints.Count(), violation!.Violations.Count);
    }

    private IComputationConstraint<IReadOnlyComputationState> CreateInvalidConstraintMock<TState, TSymbol>()
    {
        var constraint = new Mock<IComputationConstraint<IReadOnlyComputationState>>();
        constraint
            .Setup(sc => sc.Enforce(It.IsAny<IReadOnlyComputationState>()))
            .Returns(() => new ConstraintViolation("Mock for testing."));

        return constraint.Object;
    }
}
