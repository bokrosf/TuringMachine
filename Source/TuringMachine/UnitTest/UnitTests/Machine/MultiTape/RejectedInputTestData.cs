using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Tests.UnitTests.Machine.MultiTape;

internal class RejectedInputTestData : TestData
{
	public override IEnumerator<object[]> GetEnumerator()
	{
		yield return GetSingleStepData();
		yield return GetMultipleStepData();
	}

	private object[] GetSingleStepData()
	{
		var input = "a".Select(c => new Symbol<char>(c));
		var transitions = new Transition<int, char>[]
		{
			new(
				(State<int>.Initial, State<int>.Accept),
				new TapeTransition<char>[] { ('b', 'b', TapeHeadDirection.Stay), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) })
		};

		var transitionTable = new TransitionTable<int, char>(transitions);

		return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
	}

	private object[] GetMultipleStepData()
	{
		var input = "aaaabbbaaaa".Select(c => new Symbol<char>(c));
		var transitions = new Transition<int, char>[]
		{
			new(
				(State<int>.Initial, 1),
				new TapeTransition<char>[] { ('a', 'a', TapeHeadDirection.Stay), ('a', 'a', TapeHeadDirection.Right) }),
			new(
				(1, 1),
				new TapeTransition<char>[] { ('a', 'a', TapeHeadDirection.Right), ('a', 'a', TapeHeadDirection.Right) }),
			new(
				(1, State<int>.Accept),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay), ('b', 'b', TapeHeadDirection.Stay) })
		};

		var transitionTable = new TransitionTable<int, char>(transitions);

		return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
	}
}