using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape;

internal class AcceptedInputTestData : TestData
{
    public override IEnumerator<object[]> GetEnumerator()
    {
        yield return GetSingleStepData();
        yield return GetSameSymbolReadMultipleTimesData();
        yield return GetMultipleSymbolReadAndStateChangeData();
    }

    private object[] GetSingleStepData()
    {
        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, 'a'), (State<int>.Accept, 'b', TapeHeadDirection.Stay))
        };

        var transitionTable = new TransitionTable<int, char>(transitions);
        var input = "a".Select(c => new Symbol<char>(c));

        return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
    }

    private object[] GetSameSymbolReadMultipleTimesData()
    {
        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, 'a'), (1, 'a', TapeHeadDirection.Right)),
                ((1, 'a'), (1, 'a', TapeHeadDirection.Right)),
                ((1, 'b'), (State<int>.Accept, 'b', TapeHeadDirection.Stay)),
        };

        var transitionTable = new TransitionTable<int, char>(transitions);
        var input = "aaaaaaab".Select(c => new Symbol<char>(c));

        return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
    }

    private object[] GetMultipleSymbolReadAndStateChangeData()
    {
        var transitions = new Transition<int, char>[]
        {
                ((State<int>.Initial, 'a'), (1, 'a', TapeHeadDirection.Right)),
                ((1, 'b'), (2, 'c', TapeHeadDirection.Stay)),
                ((2, 'c'), (1, 'c', TapeHeadDirection.Right)),
                ((1, 'd'), (State<int>.Accept, 'e', TapeHeadDirection.Stay)),
        };

        var transitionTable = new TransitionTable<int, char>(transitions);
        var input = "abbbbbd".Select(c => new Symbol<char>(c));

        return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
    }
}
