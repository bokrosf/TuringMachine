﻿using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine.Computation.SingleTape;

/// <summary>
/// Represents the state of a single-tape machine's computation.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class ComputationState<TState, TSymbol> : Computation.ComputationState<TransitionDomain<TState, TSymbol>>
{
    public override TransitionDomain<TState, TSymbol> Configuration { get; protected set; }

    /// <summary>
    /// Initialzes a new instance of <see cref="ComputationState{TState, TSymbol}"/> class to be in <see cref="State{T}.Initial"/> state
    /// and to contain the specified symbol.
    /// </summary>
    public ComputationState(Symbol<TSymbol> symbol)
    {
        Configuration = (State<TState>.Initial, symbol);
    }

    protected override bool IsFinishState(TransitionDomain<TState, TSymbol> configuration)
    {
        return configuration.State.IsFinishState;
    }
}
