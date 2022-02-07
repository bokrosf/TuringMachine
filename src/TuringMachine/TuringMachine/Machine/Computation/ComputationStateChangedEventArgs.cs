using System;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents the base class for classes that provides data for the event that is raised when a machine's computation state changed.
/// </summary>
public abstract class ComputationStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// The number of steps have taken since the start of the computation.
    /// </summary>
    public int StepCount { get; }

    /// <summary>
    /// The elapsed time since the start of the computation.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ComputationStateChangedEventArgs"/> class with the given computation state.
    /// </summary>
    /// <param name="computationState">State of the computation.</param>
    protected ComputationStateChangedEventArgs(IReadOnlyComputationState computationState)
    {
        StepCount = computationState.StepCount;
        Duration = computationState.Duration;
    }
}
