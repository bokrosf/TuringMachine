namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Enforces the defined constraint during computation.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface IComputationConstraint<TState, TSymbol>
{
    /// <summary>
    /// Enforces the constraint.
    /// </summary>
    /// <param name="computationState">Computation state that the constraint apply to.</param>
    ConstraintViolation? Enforce(IReadOnlyComputationState<TState, TSymbol> computationState);
}
