using System;
using System.Collections.Generic;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Provides data for the event that is raised when a computation has been aborted.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class ComputationAbortedEventArgs<TState, TSymbol> : ComputationTerminatedEventArgs<TState, TSymbol>
    {
        /// <summary>
        /// The exception that caused the abortion.
        /// </summary>
        public ComputationAbortedException Exception { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortedEventArgs{TState, TSymbol}"/> class with the given step count, duration, state 
        /// the resulting symbols of the computation and the cause of abortion.
        /// </summary>
        /// <param name="stepCount">The number of steps have taken since the start of the computation.</param>
        /// <param name="duration">The elapsed time since the start of the computation.</param>
        /// <param name="state">The state that the machine terminated at.</param>
        /// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
        /// <param name="exception">The exception that caused the abortion.</param>
        public ComputationAbortedEventArgs(
            int stepCount, 
            TimeSpan duration, 
            State<TState> state, 
            IEnumerable<Symbol<TSymbol>> result,
            ComputationAbortedException exception)
            : base(stepCount, duration, state, result)
        {
            Exception = exception;
        }
    }
}
