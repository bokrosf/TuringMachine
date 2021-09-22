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
        /// Initializes a new instance of <see cref="ComputationTerminatedEventArgs{TState, TSymbol}"/> class with the specified computation state
        /// and the resulting symbols of the computation.
        /// </summary>
        /// <param name="computationState">State of the computation.</param>
        /// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
        public ComputationTerminatedEventArgs(IReadOnlyComputationState<TState, TSymbol> computationState, IEnumerable<Symbol<TSymbol>> result)
            : base(computationState.StepCount, computationState.Duration)
        {
            State = computationState.Configuration.State;
            Result = result.ToList().AsReadOnly();
        }
    }
}
