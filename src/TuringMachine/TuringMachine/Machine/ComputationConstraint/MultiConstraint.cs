using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Ensures all the given constraints.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class MultiConstraint<TState, TSymbol> : ComputationConstraint<TState, TSymbol>
    {
        private readonly IReadOnlyCollection<ComputationConstraint<TState, TSymbol>> constraints;

        /// <summary>
        /// Initializes a new instance of <see cref="MultiConstraint{TState, TSymbol}"/> class with the specified constraints.
        /// </summary>
        /// <param name="constraints">Constraints need to be enforced.</param>
        public MultiConstraint(IEnumerable<ComputationConstraint<TState, TSymbol>> constraints)
        {
            if (!constraints.Any())
            {
                throw new ArgumentException("Collection must contain at least one constraint.", nameof(constraints));
            }

            this.constraints = constraints.ToList().AsReadOnly();
        }

        /// <inheritdoc/>
        /// <exception cref="ComputationAbortedException">Any of the constraints could not be enforced.</exception>
        public override void Enforce(IReadOnlyComputationState<TState, TSymbol> computationState)
        {
            if (IsComputationFinished(computationState))
            {
                return;
            }

            foreach (var c in constraints)
            {
                c.Enforce(computationState);
            }
        }
    }
}
