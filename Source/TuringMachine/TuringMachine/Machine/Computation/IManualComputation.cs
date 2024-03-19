using System.Collections.Generic;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for controlling manual computations.
/// </summary>
/// <typeparam name="TComputationRequest">Arguments of a computation initiation.</typeparam>
public interface IManualComputation<TComputationRequest> where TComputationRequest : notnull
{
    /// <summary>
    /// Starts a manually steppable computation process.
    /// </summary>
    /// <param name="request">Arguments of a computation initiation.</param>
    void StartManual(TComputationRequest request);

    /// <summary>
    /// Transitions the machine from it's current state to the next.
    /// </summary>
    /// <returns><see cref="bool"/> that indicates whether another step can be made after the current step performed.</returns>
    bool Step();

    /// <summary>
    /// Aborts the computation that is in progress.
    /// </summary>
    void RequestAbortion();
}
