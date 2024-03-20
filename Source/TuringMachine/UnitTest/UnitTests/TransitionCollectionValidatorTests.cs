using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Tests.UnitTests.Transition;
using TuringMachine.Transition;
using TuringMachine.Transition.SingleTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests;

public class TransitionCollectionValidatorTests
{
    private readonly TransitionCollectionValidator<string, int> validator;

    public TransitionCollectionValidatorTests()
    {
        validator = new TransitionCollectionValidator<string, int>();
    }

    [Fact]
    public void Validate_ValidCollection()
    {
        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, new Symbol<int>(1)), ("q1", 2, TapeHeadDirection.Right)),
            (("q1", 2), ("q1", 3, TapeHeadDirection.Stay)),
            (("q1", 3), (State<string>.Accept, new Symbol<int>(4), TapeHeadDirection.Stay)),
        };

        validator.Validate(transitions);
    }

    [Fact]
    public void Validate_EmptyCollection_Invalid()
    {
        var transitions = Enumerable.Empty<Transition<string, int>>();

        Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
    }

    [Fact]
    public void Validate_MissingInitialState_Invalid()
    {
        var transitions = new Transition<string, int>[]
        {
            (("q1", 2), ("q2", 3, TapeHeadDirection.Stay)),
            (("q2", 3), (State<string>.Accept, 3, TapeHeadDirection.Stay))
        };

        Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
    }

    [Fact]
    public void Validate_MissingAcceptState_Invalid()
    {
        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, 3), ("q2", 3, TapeHeadDirection.Stay)),
            (("q2", 3), ("q2", 3, TapeHeadDirection.Stay))
        };

        Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
    }

    [Fact]
    public void Validate_MissingAllSpecialStates_Invalid()
    {
        var transitions = new Transition<string, int>[]
        {
            (("q2", 3), ("q2", 3, TapeHeadDirection.Stay))
        };

        Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
    }

    [Fact]
    public void Validate_NonDeterministic_Invalid()
    {
        var domain = (State<string>.Initial, 1);

        var transitions = new Transition<string, int>[]
        {
            (domain, ("q1", 2, TapeHeadDirection.Right)),
            (domain, (State<string>.Accept, 4, TapeHeadDirection.Stay)),
        };

        Assert.Throws<NonDeterministicTransitionException>(() => validator.Validate(transitions));
    }

    [Theory]
    [ClassData(typeof(InvalidDomainStateTestData<string>))]
    public void Validate_InvalidDomainState_Invalid(State<string> invalidDomainState)
    {
        var invalidTransition = new Transition<string, int>(
            (invalidDomainState, new Symbol<int>(2)),
            ("q1", 2, TapeHeadDirection.Right));

        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, 1), ("q1", 3, TapeHeadDirection.Left)),
            invalidTransition,
            (("qEnd", 2), (State<string>.Accept, 3, TapeHeadDirection.Left)),
        };

        Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
    }

    [Theory]
    [ClassData(typeof(InvalidRangeStateTestData<string>))]
    public void Validate_InvalidRangeState_Invalid(State<string> invalidRangeState)
    {
        var transition = new Transition<string, int>(
            ("q1", 1),
            (invalidRangeState, new Symbol<int>(2), TapeHeadDirection.Right));

        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, 1), ("q1", 3, TapeHeadDirection.Left)),
            transition,
            (("qEnd", 2), (State<string>.Accept, 3, TapeHeadDirection.Left)),
        };

        Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
    }
}
