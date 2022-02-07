using System;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Notifies clients that computation state has changed.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface IComputationTracking<TState, TSymbol, TTransition>
    where TTransition : notnull
{
    /// <summary>
    /// Occures when a machine transitions from one state to another.
    /// </summary>
    event EventHandler<SteppedEventArgs<TTransition>>? Stepped;

    /// <summary>
    /// Occures when a computation terminated.
    /// </summary>
    event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;

    /// <summary>
    /// Occures when a computation aborted.
    /// </summary>
    event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;
}
