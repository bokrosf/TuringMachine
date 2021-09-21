using System;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents the state of a computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class ComputationState<TState, TSymbol>
    {
        /// <summary>
        /// Current configuration of the machine.
        /// </summary>
        public TransitionDomain<TState, TSymbol> Configuration { get; set; }
        
        /// <summary>
        /// Count of steps taken since the start of the computation.
        /// </summary>
        public int StepCount { get; set; }
        
        /// <summary>
        /// Elapsed time since the start of the computation.
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// Initialzes a new instance of <see cref="ComputationState{TState, TSymbol}"/> class with the specified configuration, step count
        /// and elapsed time.
        /// </summary>
        /// <param name="configuration">Current configuration of the machine.</param>
        /// <param name="stepCount">Count of steps taken since the start of the computation.</param>
        /// <param name="elapsedTime">Elapsed time since the start of the computation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Step count less than zero or elapsed time less than <see cref="TimeSpan.Zero"/>.</exception>
        public ComputationState(TransitionDomain<TState, TSymbol> configuration, int stepCount, TimeSpan elapsedTime)
        {
            if (stepCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepCount), stepCount, $"Step count must be greater than or equal to zero.");
            }

            if (elapsedTime < TimeSpan.Zero)
            {
                string message = $"Elapsed time must be greater than or equal to {nameof(TimeSpan)}.{nameof(TimeSpan.Zero)}";
                throw new ArgumentOutOfRangeException(nameof(elapsedTime), elapsedTime, message);
            }

            Configuration = configuration;
            StepCount = stepCount;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Returns a read-only wrapper for the current instance.
        /// </summary>
        /// <returns>An object that acts as a read-only wrapper around the current <see cref="ComputationState{TState, TSymbol}"/>.</returns>
        public ReadOnlyComputationState<TState, TSymbol> AsReadOnly() => new ReadOnlyComputationState<TState, TSymbol>(this);
    }
}
