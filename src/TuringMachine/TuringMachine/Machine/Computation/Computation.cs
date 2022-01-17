using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Stores information about a computation.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <param name="Mode">Which mode the computation has been started.</param>
/// <param name="State">Current state of the computation.</param>
/// <param name="Constraint">Constraint that must be enforced during the computation.</param>
/// <param name="IsAborted">whether the computation has aborted</param>
internal record Computation<TState, TSymbol>(
    ComputationMode Mode,
    ComputationState<TState, TSymbol> State,
    IComputationConstraint<TState, TSymbol>? Constraint,
    bool IsAborted);
