using System;
using TuringMachine.Transition;

namespace TuringMachine.Machine.Computation
{
    /// <summary>
    /// Represents a read-only computation state.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IReadOnlyComputationState<TState, TSymbol>
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
    }
}
