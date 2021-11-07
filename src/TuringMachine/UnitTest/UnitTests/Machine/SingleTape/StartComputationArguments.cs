using System.Collections.Generic;
using TuringMachine.Transition;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    public record StartComputationArguments<TState, TSymbol>(TransitionTable<TState, TSymbol> TransitionTable, IEnumerable<Symbol<TSymbol>> Input);
}
