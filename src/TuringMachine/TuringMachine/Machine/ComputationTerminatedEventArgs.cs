using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine
{
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
        /// Symbols from the machine's tape after the computation has terminated.
        /// </summary>
        public IReadOnlyList<Symbol<TSymbol>> Result { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationTerminatedEventArgs"/> class with the given step count, duration state and the resulting symbols of the computation.
        /// </summary>
        /// <param name="stepCount">The number of steps have taken since the start of the computation.</param>
        /// <param name="duration">The elapsed time since the start of the computation.</param>
        /// <param name="state">The state that the machine terminated at.</param>
        /// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
        public ComputationTerminatedEventArgs(int stepCount, TimeSpan duration, State<TState> state, IEnumerable<Symbol<TSymbol>> result)
            : base(stepCount, duration)
        {
            State = state;
            Result = result.ToList().AsReadOnly();
        }
    }
}
