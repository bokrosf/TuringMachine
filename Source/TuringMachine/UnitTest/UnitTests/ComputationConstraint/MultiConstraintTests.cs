using NSubstitute;
using System;
using System.Linq;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint;

public class MultiConstraintTests
{
    private readonly IReadOnlyComputationState computationState;
    
    public MultiConstraintTests()
    {
        computationState = Substitute.For<IReadOnlyComputationState>();
    }
    
    [Fact]
    public void Constructor_EmptyConstraintList_ThrowsException()
    {
        var contraints = Enumerable.Empty<IComputationConstraint<IReadOnlyComputationState>>();
        Assert.Throws<ArgumentException>(() => new MultiConstraint(contraints));
    }

    [Fact]
    public void Enforce_SingleValidConstraint_NotThrowsException()
    {
        var singleConstraint = Substitute.For<IComputationConstraint<IReadOnlyComputationState>>();
        var constraints = new IComputationConstraint<IReadOnlyComputationState>[] { singleConstraint };
        var multiConstraint = new MultiConstraint(constraints);

        multiConstraint.Enforce(computationState);
    }

    [Fact]
    public void Enforce_AllConstraintsValid_NotThrowsException()
    {
        var constraints = Enumerable.Range(0, 5).Select(i => Substitute.For<IComputationConstraint<IReadOnlyComputationState>>());
        var multiConstraint = new MultiConstraint(constraints);

        multiConstraint.Enforce(computationState);
    }

    [Fact]
    public void Enforce_SingleInvalidConstraint_ThrowsException()
    {
        var constraints = new IComputationConstraint<IReadOnlyComputationState>[] { CreateInvalidConstraint<int, char>() };
        var multiConstraint = new MultiConstraint(constraints);

        var violation = multiConstraint.Enforce(computationState) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(1, violation!.Violations.Count);
    }

    [Fact]
    public void Enforce_AllConstraintsInvalid_ThrowsException()
    {
        var constraints = Enumerable.Range(0, 5).Select(i => CreateInvalidConstraint<int, char>());
        var multiConstraint = new MultiConstraint(constraints);

        var violation = multiConstraint.Enforce(computationState) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(constraints.Count(), violation!.Violations.Count);
    }

    [Fact]
    public void Enforce_MixedValidityConstraints_ThrowsException()
    {
        var validConstraints = Enumerable.Range(0, 5).Select(i => Substitute.For<IComputationConstraint<IReadOnlyComputationState>>());
        var invalidConstraints = Enumerable.Range(0, 5).Select(i => CreateInvalidConstraint<int, char>());
        var multiConstraint = new MultiConstraint(validConstraints.Concat(invalidConstraints));

        var violation = multiConstraint.Enforce(computationState) as MultiViolation;

        Assert.NotNull(violation);
        Assert.Equal(invalidConstraints.Count(), violation!.Violations.Count);
    }

    private IComputationConstraint<IReadOnlyComputationState> CreateInvalidConstraint<TState, TSymbol>()
    {
        var constraint = Substitute.For<IComputationConstraint<IReadOnlyComputationState>>();
        constraint
            .Enforce(Arg.Any<IReadOnlyComputationState>())
            .Returns(_ => new ConstraintViolation("Mock for testing."));

        return constraint;
    }
}
