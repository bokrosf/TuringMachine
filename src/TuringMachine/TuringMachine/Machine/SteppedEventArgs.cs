using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Provides data for the event that is raised when a turing machine transitioned from one state to another.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class SteppedEventArgs<TState, TSymbol> : ComputationStateChangedEventArgs
    {
        /// <summary>
        /// The applied transition during the step.
        /// </summary>
        public Transition<TState, TSymbol> Transition { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="SteppedEventArgs{TState, TSymbol}"/> class with the the specified computation state
        /// and transition.
        /// </summary>
        /// <param name="computationState">State of the computation.</param>
        /// <param name="transition">The applied transition during the step.</param>
        public SteppedEventArgs(IReadOnlyComputationState<TState, TSymbol> computationState, Transition<TState, TSymbol> transition)
            : base(computationState.StepCount, computationState.Duration)
        {
            Transition = transition;
        }
    }
}
