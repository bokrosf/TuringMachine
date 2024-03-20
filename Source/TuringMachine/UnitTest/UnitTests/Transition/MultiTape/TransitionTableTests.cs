using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;
using TuringMachine.Transition.MultiTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Transition.MultiTape;

public class TransitionTableTests
{
	[Fact]
	public void Indexer_ExistingDomain_ReturnsRange()
	{
		var existingTransition = new Transition<string, int>(
			("q1", "q2"),
			new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) });

		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			existingTransition,
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		var transitionTable = new TransitionTable<string, int>(transitions);
		var domain = new TransitionDomain<string, int>(existingTransition.State.Domain, existingTransition.Tapes.Select(tape => tape.Domain));
		var expectedRange = new TransitionRange<string, int>(
			existingTransition.State.Range,
			existingTransition.Tapes.Select(tape => new TapeTransitionRange<int>(tape.Range, tape.TapeHeadDirection)));

		var foundRange = transitionTable[domain];

		Assert.Equal(expectedRange.State, foundRange.State);
		Assert.Equal(expectedRange.Tapes, foundRange.Tapes);
	}

	[Fact]
	public void Indexer_NotExistingDomain_ThrowsException()
	{
		var existingDomainStateName = "q1";
		var existingTransition = new Transition<string, int>(
			(existingDomainStateName, "q2"),
			new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) });

		Transition<string, int>[] transitions =
		{
			new(
				(State<string>.Initial, "q1"),
				new TapeTransition<int>[] { (1, 2, TapeHeadDirection.Right), (1, 3, TapeHeadDirection.Stay), (1, 4, TapeHeadDirection.Left) }),
			existingTransition,
			new(
				("q2", State<string>.Accept),
				new TapeTransition<int>[] { (2, 3, TapeHeadDirection.Right), (3, 3, TapeHeadDirection.Stay), (4, 6, TapeHeadDirection.Left) })
		};

		var notExistingDomain = new TransitionDomain<string, int>(new State<string>(existingDomainStateName + "diff"), existingTransition.Tapes.Select(tape => tape.Domain));
		var transitionTable = new TransitionTable<string, int>(transitions);

		Assert.Throws<TransitionDomainNotFoundException>(() => transitionTable[notExistingDomain]);
	}
}
