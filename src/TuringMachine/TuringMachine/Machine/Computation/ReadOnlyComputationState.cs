using System;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents a machine's read-only computation state.
/// </summary>
/// <typeparam name="TConfiguration">Type of the machine's configuration.</typeparam>
internal class ReadOnlyComputationState<TConfiguration> : IReadOnlyComputationState<TConfiguration>
{
    private readonly IReadOnlyComputationState<TConfiguration> computationState;

    /// <summary>
    /// Initializes a new instance of <see cref="ReadOnlyComputationState{TState, TConfiguration}"/> class with the specified computation state.
    /// </summary>
    /// <param name="computationState">State of a computation.</param>
    public ReadOnlyComputationState(IReadOnlyComputationState<TConfiguration> computationState)
    {
        this.computationState = computationState;
    }

    public TConfiguration Configuration => computationState.Configuration;
    
    public int StepCount => computationState.StepCount;
    
    public TimeSpan Duration => computationState.Duration;
}
