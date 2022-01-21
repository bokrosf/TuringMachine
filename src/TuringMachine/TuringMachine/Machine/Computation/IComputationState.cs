using System;
using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents the state of a computation.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface IComputationState<TState, TSymbol> : IReadOnlyComputationState<TState, TSymbol>
{
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
