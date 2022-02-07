using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Provides data for the event that is raised when a computation has been terminated.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class ComputationTerminatedEventArgs<TState, TSymbol> : ComputationStateChangedEventArgs
{
    /// <summary>
    /// The state that the machine terminated at.
    /// </summary>
    public State<TState> State { get; }

    /// <summary>
    /// Symbols from the machine's tape after the computation has terminated. It can contain <see cref="Symbol{T}.Blank"/> symbols.
    /// </summary>
    public IReadOnlyList<Symbol<TSymbol>> RawResult { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ComputationTerminatedEventArgs{TState, TSymbol}"/> class with the specified computation state, 
    /// state the machine terminated at and the resulting symbols of the computation.
    /// </summary>
    /// <param name="computationState">State of a computation.</param>
    /// <param name="state">The state that the machine terminated at.</param>
    /// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
    public ComputationTerminatedEventArgs(
        IReadOnlyComputationState computationState, 
        State<TState> state, 
        IEnumerable<Symbol<TSymbol>> result)
        : base(computationState)
    {
        State = state;
        RawResult = result.ToList().AsReadOnly();
    }

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
        int indexFrom = StepThroughBlankSymbols(0, i => i < RawResult.Count, i => ++i);
        int indexTo = StepThroughBlankSymbols(RawResult.Count - 1, i => i > indexFrom, i => --i);

        for (int i = indexFrom; i <= indexTo && i < RawResult.Count; i++)
        {
            yield return RawResult[i];
        }
    }

    private int StepThroughBlankSymbols(int indexFrom, Predicate<int> canStepIndex, Func<int, int> indexStepper)
    {
        int i = indexFrom;

        while (canStepIndex(i) && RawResult[i] == Symbol<TSymbol>.Blank)
        {
            i = indexStepper(i);
        }

        return i;
    }
}
