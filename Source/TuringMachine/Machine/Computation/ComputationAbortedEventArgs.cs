using System;
using System.Collections.Generic;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Provides data for the event that is raised when a computation has been aborted.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class ComputationAbortedEventArgs<TState, TSymbol> : ComputationTerminatedEventArgs<TState, TSymbol>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ComputationAbortedEventArgs{TState, TSymbol}"/> class with the specified 
	/// computation state the machine was at abortion, the resulting symbols of the computation and the cause of abortion.
	/// </summary>
	/// <param name="state">The state that the machine terminated at.</param>
	/// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
	/// <param name="exception">The exception that caused the abortion.</param>
	public ComputationAbortedEventArgs(State<TState> state, IEnumerable<Symbol<TSymbol>> result, Exception? exception)
		: base(state, result)
	{
		Exception = exception;
	}

	/// <summary>
	/// The exception that caused the abortion.
	/// </summary>
	public Exception? Exception { get; }
}
