using System;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents a read-only computation state.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class ReadOnlyComputationState<TState, TSymbol> : IReadOnlyComputationState<TState, TSymbol>
    {
        private readonly IComputationState<TState, TSymbol> computationState;

        /// <summary>
        /// Current configuration of the machine.
        /// </summary>
        public TransitionDomain<TState, TSymbol> Configuration => computationState.Configuration;

        /// <summary>
        /// Steps taken since the start of the computation.
        /// </summary>
        public int StepCount => computationState.StepCount;

        /// <summary>
        /// Elapsed time since the start of the computation.
        /// </summary>
        public TimeSpan Duration => computationState.Duration;

        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyComputationState{TState, TSymbol}"/> class with the specified computation state.
        /// </summary>
        /// <param name="computationState">State of a computation.</param>
        public ReadOnlyComputationState(IComputationState<TState, TSymbol> computationState)
        {
            this.computationState = computationState;
        }
    }
}
