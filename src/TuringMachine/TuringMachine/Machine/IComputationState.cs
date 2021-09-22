using System;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents the state of a computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IComputationState<TState, TSymbol>
    {
        /// <summary>
        /// Current configuration of the machine.
        /// </summary>
        TransitionDomain<TState, TSymbol> Configuration { get; }

        /// <summary>
        /// Steps taken since the start of the computation.
        /// </summary>
        int StepCount { get; }

        /// <summary>
        /// Elapsed time since the start of the computation.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Updates the configuration to the specified one.
        /// </summary>
        /// <param name="configuration">Configuration of the matchine.</param>
        void UpdateConfiguration(TransitionDomain<TState, TSymbol> configuration);

        /// <summary>
        /// Starts recording the elapsed time since the start of the computation.
        /// </summary>
        void StartDurationWatch();

        /// <summary>
        /// Stops recording the elapsed time since the start of the computation.
        /// </summary>
        void StopDurationWatch();
    }
}
