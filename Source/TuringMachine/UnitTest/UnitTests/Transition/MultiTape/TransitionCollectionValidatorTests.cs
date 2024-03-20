using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;
using TuringMachine.Transition.MultiTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Transition.MultiTape;

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
        Transition<string, int>[] transitions =
        {
            new(
                (State<string>.Initial, "q1"),
                new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
            new(
                ("q1", "q2"),
                new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) }),
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
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
		Transition<string, int>[] transitions =
		{
			new(
				("q1", "q2"),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) }),
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
	}

	[Fact]
	public void Validate_MissingAcceptState_Invalid()
	{
		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			new(
				("q1", "q2"),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
	}

	[Fact]
	public void Validate_MissingAllSpecialStates_Invalid()
	{
		Transition<string, int>[] transitions =
		{
			new(
				("q1", "q2"),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<MissingStateException>(() => validator.Validate(transitions));
	}

	[Fact]
	public void Validate_NonDeterministic_Invalid()
	{
		State<string> stateDomain = new State<string>("q1");
		Symbol<int> symbolDomain = new Symbol<int>(2);
		
		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			new(
				(stateDomain, "q2"),
				new TapeTransition<int>[] { (symbolDomain, 3, TapeHeadDirection.Right), (symbolDomain, 3, TapeHeadDirection.Stay), (symbolDomain, 6, TapeHeadDirection.Left) }),
			new(
				(stateDomain, "q3"),
				new TapeTransition<int>[] { (symbolDomain, 99, TapeHeadDirection.Right), (symbolDomain, 99, TapeHeadDirection.Stay), (symbolDomain, 6, TapeHeadDirection.Left) }),
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<NonDeterministicTransitionException>(() => validator.Validate(transitions));
	}

	[Theory]
	[ClassData(typeof(InvalidDomainStateTestData<string>))]
	public void Validate_InvalidDomainState_Invalid(State<string> invalidDomainState)
	{
		var invalidTransition = new Transition<string, int>(
			(invalidDomainState, "q2"),
			new TapeTransition<int>[] { (99, 3, TapeHeadDirection.Right), (99, 3, TapeHeadDirection.Stay), (99, 6, TapeHeadDirection.Left) });

		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			invalidTransition,
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
	}

	[Theory]
	[ClassData(typeof(InvalidRangeStateTestData<string>))]
	public void Validate_InvalidRangeState_Invalid(State<string> invalidRangeState)
	{
		var invalidTransition = new Transition<string, int>(
			("q1", invalidRangeState),
			new TapeTransition<int>[] { (99, 3, TapeHeadDirection.Right), (99, 3, TapeHeadDirection.Stay), (99, 6, TapeHeadDirection.Left) });

		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			invalidTransition,
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
	}

	[Fact]
	public void Validate_DifferentTapeCount_Invalid()
	{
		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			new(
				("q1", "q2"),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) }),
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right) })
		};

		Assert.Throws<DifferentTransitionTapeCountException>(() => validator.Validate(transitions));
	}
}
