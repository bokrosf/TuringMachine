namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Enforces the defined constraint during computation.
/// </summary>
/// <typeparam name="TComputationState">Type of the computation's state.</typeparam>
public interface IComputationConstraint<in TComputationState> 
    where TComputationState : IReadOnlyComputationState
{
    /// <summary>
    /// Enforces the constraint.
    /// </summary>
    /// <param name="computationState">Computation state that the constraint applies to.</param>
    ConstraintViolation? Enforce(TComputationState computationState);
}
