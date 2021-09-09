using System;
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
        /// Initializes a new instance of <see cref="SteppedEventArgs{TState, TSymbol}"/> class with the given step count, duration and transition.
        /// </summary>
        /// <param name="stepCount">The number of steps have taken since the start of the computation.</param>
        /// <param name="duration">The elapsed time since the start of the computation.</param>
        /// <param name="transition">The applied transition during the step.</param>
        public SteppedEventArgs(int stepCount, TimeSpan duration, Transition<TState, TSymbol> transition)
            : base(stepCount, duration)
        {
            Transition = transition;
        }
    }
}
