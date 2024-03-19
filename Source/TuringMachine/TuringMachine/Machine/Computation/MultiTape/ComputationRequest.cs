using System.Collections.Generic;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Machine.Computation.MultiTape;

/// <summary>
/// Arguments of a multi-tape computation initiation.
/// </summary>
/// <param name="Input">Symbols that the tape is initialized with.</param>
/// <param name="TransitionTable">Table that contains the performable transitions.</param>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public record ComputationRequest<TState, TSymbol>(IEnumerable<Symbol<TSymbol>> Input, ITransitionTable<TState, TSymbol> TransitionTable)
    : ComputationRequest<TSymbol>(Input);