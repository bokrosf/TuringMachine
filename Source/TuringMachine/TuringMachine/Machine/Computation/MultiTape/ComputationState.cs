using System.Collections.Generic;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Machine.Computation.MultiTape;

/// <summary>
/// Represents the state of a multi-tape machine's computation.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class ComputationState<TState, TSymbol> : Computation.ComputationState<TransitionDomain<TState, TSymbol>>
{
    public override TransitionDomain<TState, TSymbol> Configuration { get; protected set; }

    /// <summary>
    /// Creates a new instance of <see cref="ComputationState{TState, TSymbol}"/> class with the specified symbols per tape.
    /// </summary>
    /// <param name="symbols">Current symbols on each tape.</param>
    public ComputationState(IEnumerable<Symbol<TSymbol>> symbols)
    {
        Configuration = new TransitionDomain<TState, TSymbol>(State<TState>.Initial, symbols);
    }

    protected override bool IsFinishState(TransitionDomain<TState, TSymbol> configuration)
    {
        return configuration.State.IsFinishState;
    }
}
