namespace TuringMachine.Machine.Computation;

using System.Collections.Generic;

/// <summary>
/// Arguments of a computation initiation.
/// </summary>
/// <param name="Input">Symbols that the tape is initialized with.</param>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public abstract record ComputationRequest<TSymbol>(IEnumerable<Symbol<TSymbol>> Input);