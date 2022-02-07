using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Stores information about a computation.
/// </summary>
/// <typeparam name="TComputationState">Type of the computation's state.</typeparam>
/// <typeparam name="TConfiguration">Type of the machine's configuration</typeparam>
/// <param name="Mode">Which mode the computation has been started.</param>
/// <param name="State">Current state of the computation.</param>
/// <param name="Constraint">Constraint that must be enforced during the computation.</param>
/// <param name="IsAborted">whether the computation has aborted</param>
public record Computation<TComputationState, TConfiguration>(
    ComputationMode Mode,
    TComputationState State,
    IComputationConstraint<IReadOnlyComputationState<TConfiguration>>? Constraint,
    bool IsAborted)
        where TComputationState : IReadOnlyComputationState
        where TConfiguration : notnull;
