using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Base class for defining constraints that must be enforced during computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public abstract class ComputationConstraint<TState, TSymbol>
    {
        /// <summary>
        /// Checks whether the constraint is enforced.
        /// </summary>
        /// <param name="computationState">Computation state that the constraint apply to.</param>
        /// <exception cref="ComputationAbortedException">Rule enforcement failed.</exception>
        public abstract void Enforce(IReadOnlyComputationState<TState, TSymbol> computationState);

        /// <summary>
        /// Returns whether computation has been finished.
        /// </summary>
        /// <param name="computationState">Computation state.</param>
        /// <returns>true if the computation has finished already; otherwise, false.</returns>
        protected bool IsComputationFinished(IReadOnlyComputationState<TState, TSymbol> computationState)
        {
            return GetFinishedStates().Contains(computationState.Configuration.State);
        }

        private IEnumerable<State<TState>> GetFinishedStates()
        {
            yield return State<TState>.Accept;
            yield return State<TState>.Reject;
        }
    }
}
