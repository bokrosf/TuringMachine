using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents the range of a multi-tape machine transition.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
internal class TransitionRange<TState, TSymbol>
{
    /// <summary>
    /// The state that the machine enters after the transition.
    /// </summary>
    public State<TState> State { get; }

    /// <summary>
    /// Effect of the transition for each tape of the machine.
    /// </summary>
    public IReadOnlyList<TapeTransitionRange<TSymbol>> Tapes { get; }

    /// <summary>
    /// Creates a new instance of <see cref="TransitionRange{TState, TSymbol}"/> class with the specified state
    /// and the effect of the transition for each tape.
    /// </summary>
    /// <param name="state">The state that the machine enters after the transition.</param>
    /// <param name="tapes">Effect of the transition for each tape of the machine.</param>
    public TransitionRange(State<TState> state, IEnumerable<TapeTransitionRange<TSymbol>> tapes)
    {
        State = state;
        Tapes = tapes.ToList().AsReadOnly();
    }
}
