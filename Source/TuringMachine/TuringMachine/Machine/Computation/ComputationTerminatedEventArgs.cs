using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Provides data for the event that is raised when a computation has been terminated.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class ComputationTerminatedEventArgs<TState, TSymbol> : EventArgs
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ComputationTerminatedEventArgs{TState, TSymbol}"/> class with the specified 
	/// computation state the machine terminated at and the resulting symbols of the computation.
	/// </summary>
	/// <param name="state">The state that the machine terminated at.</param>
	/// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
	public ComputationTerminatedEventArgs(State<TState> state, IEnumerable<Symbol<TSymbol>> result)
	{
		State = state;
		RawResult = result.ToList().AsReadOnly();
	}
	
    /// <summary>
	/// The state that the machine terminated at.
	/// </summary>
	public State<TState> State { get; }

    /// <summary>
    /// Symbols from the machine's tape after the computation has terminated. It can contain <see cref="Symbol{T}.Blank"/> symbols.
    /// </summary>
    public IReadOnlyList<Symbol<TSymbol>> RawResult { get; }

    /// <summary>
    /// Enumerates the result sequence without all leading and trailing <see cref="Symbol{TSymbol}.Blank"/> symbols.
    /// </summary>
    /// <returns>
    /// <see cref="IEnumerable{T}"/> The sequence that remains after all <see cref="Symbol{TSymbol}.Blank"/> symbols removed from
    /// the start and end of the result sequence. If no <see cref="Symbol{TSymbol}.Blank"/> symbols can be trimmed from the result,
    /// the method returns the current result sequence unchanged.
    /// </returns>
    public IEnumerable<Symbol<TSymbol>> TrimResult()
    {
        bool BlankSkipper(Symbol<TSymbol> symbol) => symbol == Symbol<TSymbol>.Blank;

        return RawResult.Reverse().SkipWhile(BlankSkipper).Reverse().SkipWhile(BlankSkipper);
    }
}
