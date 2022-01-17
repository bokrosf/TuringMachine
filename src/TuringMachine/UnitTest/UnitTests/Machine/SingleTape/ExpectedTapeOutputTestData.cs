using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape;

internal class ExpectedTapeOutputTestData : TestData
{
    public override IEnumerator<object[]> GetEnumerator()
    {
        yield return GetSingleSymbolTestData();
        yield return GetSameSymbolMultipleTimesTestData();
        yield return GetMultipleSymbolsTestData();
    }

    private object[] GetSingleSymbolTestData()
    {
        var input = "a".Select(c => new Symbol<char>(c));
        var output = "b".Select(c => new Symbol<char>(c));
        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, input.First().Value), (State<int>.Accept, output.First().Value, TapeHeadDirection.Stay))
        };

        return new object[] { new ExpectedTapeOutputArguments<int, char>(new TransitionTable<int, char>(transitions), input, output) };
    }

    private object[] GetSameSymbolMultipleTimesTestData()
    {
        int inputLength = 5;
        var input = Enumerable.Range(0, inputLength).Select(i => new Symbol<char>('a'));
        var output = Enumerable.Range(0, inputLength).Select(i => new Symbol<char>('b'));
        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, input.First().Value), (1, output.First().Value, TapeHeadDirection.Right)),
                ((1, input.First().Value), (1, output.First().Value, TapeHeadDirection.Right)),
                ((1, Symbol<char>.Blank), (State<int>.Accept, Symbol<char>.Blank, TapeHeadDirection.Stay)),
        };

        return new object[] { new ExpectedTapeOutputArguments<int, char>(new TransitionTable<int, char>(transitions), input, output) };
    }

    private object[] GetMultipleSymbolsTestData()
    {
        var input = "aaaaaaaa".Select(c => new Symbol<char>(c));
        var output = "cbcbcbcb".Select(c => new Symbol<char>(c));

        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, 'a'), (1, 'a', TapeHeadDirection.Right)),
                ((1, 'a'), (2, 'b', TapeHeadDirection.Right)),
                ((2, 'a'), (1, 'a', TapeHeadDirection.Right)),
                ((1, Symbol<char>.Blank), (3, Symbol<char>.Blank, TapeHeadDirection.Left)),
                ((2, Symbol<char>.Blank), (3, Symbol<char>.Blank, TapeHeadDirection.Left)),
                ((3, 'a'), (3, 'c', TapeHeadDirection.Left)),
                ((3, 'b'), (3, 'b', TapeHeadDirection.Left)),
                ((3, Symbol<char>.Blank), (State<int>.Accept, Symbol<char>.Blank, TapeHeadDirection.Stay)),
        };

        return new object[] { new ExpectedTapeOutputArguments<int, char>(new TransitionTable<int, char>(transitions), input, output) };
    }
}
