namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Defines rules that must be enforced during a computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IComputationConstraint<TState, TSymbol>
    {
        /// <summary>
        /// Checks whether the constraint is enforced.
        /// </summary>
        /// <param name="computationState">Computation state that the rules apply to.</param>
        void Enforce(ComputationState<TState, TSymbol> computationState);
    }
}
