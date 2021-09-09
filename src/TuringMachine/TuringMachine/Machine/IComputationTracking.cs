using System;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Notifies clients that computation state has changed.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IComputationTracking<TState, TSymbol>
    {
        /// <summary>
        /// Occures when a machine transitions from one state to another.
        /// </summary>
        event EventHandler<SteppedEventArgs<TState, TSymbol>> Stepped;

        /// <summary>
        /// Occures when a computation terminated.
        /// </summary>
        event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>> ComputationTerminated;
    }
}
