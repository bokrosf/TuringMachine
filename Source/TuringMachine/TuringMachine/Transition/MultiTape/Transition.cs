using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents a transition of a multi-tape machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Transition<TState, TSymbol>
{
    /// <summary>
    /// State transition.
    /// </summary>
    public StateTransition<TState> State { get; }
    
    /// <summary>
    /// Transitions per tapes.
    /// </summary>
    public IReadOnlyList<TapeTransition<TSymbol>> Tapes { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Transition{TState, TSymbol}"/> class with the specified state and tape transitions.
    /// </summary>
    /// <param name="state">State transition.</param>
    /// <param name="tapes">Transitions per tapes.</param>
    /// <exception cref="ArgumentException">Empty tape transition collection provided.</exception>
    public Transition(StateTransition<TState> state, IEnumerable<TapeTransition<TSymbol>> tapes)
    {
        if (!tapes.Any())
        {
            throw new ArgumentException("Must contain at least one tape transition.", nameof(tapes));
        }

        State = state;
        Tapes = tapes.ToList().AsReadOnly();
    }
}
