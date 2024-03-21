using System.Collections.Generic;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Tests.UnitTests.Machine.MultiTape;

public record StartComputationArguments<TState, TSymbol>(TransitionTable<TState, TSymbol> TransitionTable, IEnumerable<Symbol<TSymbol>> Input);

public record ExpectedTapeOutputArguments<TState, TSymbol>(
	TransitionTable<TState, TSymbol> TransitionTable,
	IEnumerable<Symbol<TSymbol>> Input,
	IEnumerable<Symbol<TSymbol>> ExpectedOutput)
		: StartComputationArguments<TState, TSymbol>(TransitionTable, Input);
