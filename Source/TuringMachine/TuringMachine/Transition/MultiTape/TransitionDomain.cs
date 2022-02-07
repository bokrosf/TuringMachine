using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents the domain of a single-tape machine transition.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class TransitionDomain<TState, TSymbol>
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransitionDomain{TState, TSymbol}"/> class with the specified state and tape symbols.
    /// </summary>
    /// <param name="state">State of the domain.</param>
    /// <param name="tapeSymbols">Symbols per tape of the domain.</param>
    /// <exception cref="ArgumentException">Empty tape symbol collection provided.</exception>
    public TransitionDomain(State<TState> state, IEnumerable<Symbol<TSymbol>> tapeSymbols)
    {
        if (!tapeSymbols.Any())
        {
            throw new ArgumentException("Collection must contain at least one tape symbol.", nameof(tapeSymbols));
        }

        State = state;
        TapeSymbols = tapeSymbols.ToList().AsReadOnly();
    }

    /// <summary>
    /// State of the domain.
    /// </summary>
    public State<TState> State { get; }

    /// <summary>
    /// Symbols per tape of the domain.
    /// </summary>
    public IReadOnlyList<Symbol<TSymbol>> TapeSymbols { get; }
}
