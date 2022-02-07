using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Ensures all the given constraints.
/// </summary>
public class MultiConstraint : IComputationConstraint<IReadOnlyComputationState>
{
    private readonly IReadOnlyCollection<IComputationConstraint<IReadOnlyComputationState>> constraints;

    /// <summary>
    /// Initializes a new instance of <see cref="MultiConstraint"/> class with the specified constraints.
    /// </summary>
    /// <param name="constraints">Constraints need to be enforced.</param>
    /// <exception cref="ArgumentException">Empty constraint collection provided.</exception>
    public MultiConstraint(IEnumerable<IComputationConstraint<IReadOnlyComputationState>> constraints)
    {
        if (!constraints.Any())
        {
            throw new ArgumentException("Collection must contain at least one constraint.", nameof(constraints));
        }

        this.constraints = constraints.ToList().AsReadOnly();
    }

    public ConstraintViolation? Enforce(IReadOnlyComputationState computationState)
    {
        IEnumerable<ConstraintViolation> violations = constraints
            .Select(c => c.Enforce(computationState))
            .Where(cv => cv != null)
            .Cast<ConstraintViolation>()
            .ToList()
            .AsReadOnly();

        return violations.Any()
            ? new MultiViolation("Multiple constraints violated.", violations)
            : null;
    }
}
