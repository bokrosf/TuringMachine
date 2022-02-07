using System;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents a read-only computation state.
/// </summary>
public interface IReadOnlyComputationState
{
    /// <summary>
    /// Steps taken since the start of the computation.
    /// </summary>
    int StepCount { get; }

    /// <summary>
    /// Elapsed time since the start of the computation.
    /// </summary>
    TimeSpan Duration { get; }
}
