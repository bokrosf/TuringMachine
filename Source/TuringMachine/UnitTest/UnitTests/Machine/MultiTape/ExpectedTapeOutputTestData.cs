using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Tests.UnitTests.Machine.MultiTape;

internal class ExpectedTapeOutputTestData : TestData
{
	public override IEnumerator<object[]> GetEnumerator()
	{
		yield return GetSingleSymbolTestData();
		yield return GetInputReversionData();
	}

	private object[] GetSingleSymbolTestData()
	{
		var input = "a".Select(c => new Symbol<char>(c));
		var output = "b".Select(c => new Symbol<char>(c));
		var transitions = new Transition<int, char>[]
		{
			new(
				(State<int>.Initial, State<int>.Accept),
				new TapeTransition<char>[] { ('a', 'b', TapeHeadDirection.Stay), (Symbol<char>.Blank, 'a', TapeHeadDirection.Stay) })
		};

		return new object[] { new ExpectedTapeOutputArguments<int, char>(new TransitionTable<int, char>(transitions), input, output) };
	}

	private object[] GetInputReversionData()
	{
		var transitions = new Transition<int, char>[]
		{
			new(
				(State<int>.Initial, 1),
				new TapeTransition<char>[] { ('a', 'a', TapeHeadDirection.Stay), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
			new(
				(State<int>.Initial, 1),
				new TapeTransition<char>[] { ('b', 'b', TapeHeadDirection.Stay), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
			new(
				(1, 1),
				new TapeTransition<char>[] { ('a', 'a', TapeHeadDirection.Right), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
			new(
				(1, 1),
				new TapeTransition<char>[] { ('b', 'b', TapeHeadDirection.Right), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
			new(
				(1, 2),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Left), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
			new(
				(2, 2),
				new TapeTransition<char>[] { ('a', Symbol<char>.Blank, TapeHeadDirection.Left), (Symbol<char>.Blank, 'a', TapeHeadDirection.Right) }),
			new(
				(2, 2),
				new TapeTransition<char>[] { ('b', Symbol<char>.Blank, TapeHeadDirection.Left), (Symbol<char>.Blank, 'b', TapeHeadDirection.Right) }),
			new(
				(2, 3),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Right), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Left) }),
			new(
				(3, 3),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay), ('a', 'a', TapeHeadDirection.Left) }),
			new(
				(3, 3),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay), ('b', 'b', TapeHeadDirection.Left) }),
			new(
				(3, 4),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Right) }),
			new(
				(4, 4),
				new TapeTransition<char>[] { (Symbol<char>.Blank, 'a', TapeHeadDirection.Right), ('a', 'a', TapeHeadDirection.Right) }),
			new(
				(4, 4),
				new TapeTransition<char>[] { (Symbol<char>.Blank, 'b', TapeHeadDirection.Right), ('b', 'b', TapeHeadDirection.Right) }),
			new(
				(4, State<int>.Accept),
				new TapeTransition<char>[] { (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay), (Symbol<char>.Blank, Symbol<char>.Blank, TapeHeadDirection.Stay) }),
		};

		var transitionTable = new TransitionTable<int, char>(transitions);
		var input = "aaabbaaaaabbb".Select(c => new Symbol<char>(c));
		var expectedOutput = input.Reverse();

		return new object[] { new ExpectedTapeOutputArguments<int, char>(transitionTable, input, expectedOutput) };
	}
}

