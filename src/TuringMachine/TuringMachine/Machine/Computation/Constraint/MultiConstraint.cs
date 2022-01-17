using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.Computation.Constraint
{
    /// <summary>
    /// Ensures all the given constraints.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class MultiConstraint<TState, TSymbol> : IComputationConstraint<TState, TSymbol>
    {
        private readonly IReadOnlyCollection<IComputationConstraint<TState, TSymbol>> constraints;

        /// <summary>
        /// Initializes a new instance of <see cref="MultiConstraint{TState, TSymbol}"/> class with the specified constraints.
        /// </summary>
        /// <param name="constraints">Constraints need to be enforced.</param>
        public MultiConstraint(IEnumerable<IComputationConstraint<TState, TSymbol>> constraints)
        {
            if (!constraints.Any())
            {
                throw new ArgumentException("Collection must contain at least one constraint.", nameof(constraints));
            }

            this.constraints = constraints.ToList().AsReadOnly();
        }

        /// <inheritdoc/>
        /// <exception cref="ComputationAbortedException">Any of the constraints could not be enforced.</exception>
        public ConstraintViolation? Enforce(IReadOnlyComputationState<TState, TSymbol> computationState)
        {
            IEnumerable<ConstraintViolation> violations = constraints
                .Select(c => c.Enforce(computationState))
                .Where(cv => cv != null)
                .Cast<ConstraintViolation>()
                .ToList();

            return violations.Any() 
                ? new MultiViolation("Multiple constraints violated.", violations) 
                : null;
        }
    }
}
