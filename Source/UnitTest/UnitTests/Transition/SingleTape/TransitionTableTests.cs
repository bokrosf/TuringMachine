using TuringMachine.Machine;
using TuringMachine.Transition;
using TuringMachine.Transition.SingleTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Transition.SingleTape;

public class TransitionTableTests
{
    [Fact]
    public void Indexer_ExistingDomain_ReturnsRange()
    {
        var existingTransition = new Transition<string, int>(("q1", 2), ("q1", 3, TapeHeadDirection.Left));
        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, 1), ("q1", 2, TapeHeadDirection.Right)),
            existingTransition,
            (("q1", 3), (State<string>.Accept, 4, TapeHeadDirection.Stay)),
        };

        var transitionTable = new TransitionTable<string, int>(transitions);

        var foundRange = transitionTable[existingTransition.Domain];
        bool areEqual = foundRange.Equals(existingTransition.Range);

        Assert.True(areEqual);
    }

    [Fact]
    public void Indexer_NotExistingDomain_ThrowsException()
    {
        var existingTransition = new Transition<string, int>(("q1", 2), ("q1", 3, TapeHeadDirection.Left));
        var transitions = new Transition<string, int>[]
        {
            ((State<string>.Initial, 1), ("q1", 2, TapeHeadDirection.Right)),
            existingTransition,
            (("q1", 3), (State<string>.Accept, 4, TapeHeadDirection.Stay)),
        };

        var notExistingTransition = new Transition<string, int>(
            (existingTransition.Domain.State + "diff", existingTransition.Domain.Symbol.Value + 1),
            ("q1", 3, TapeHeadDirection.Left));

        var transitionTable = new TransitionTable<string, int>(transitions);

        Assert.ThrowsAny<TransitionDomainNotFoundException>(() => transitionTable[notExistingTransition.Domain]);
    }
}
