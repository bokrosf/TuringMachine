using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Violation of a computation constraint.
/// </summary>
/// <param name="Reason">Reason of the violation.</param>
public record ConstraintViolation(string Reason);

/// <summary>
/// Step limit constraint violation.
/// </summary>
/// <param name="Reason">Reason of the violation.</param>
/// <param name="StepLimit">Maximum number of steps a computation can take.</param>
public record StepLimitViolation(string Reason, int StepLimit)
    : ConstraintViolation(Reason);

/// <summary>
/// Time limit constraint violation.
/// </summary>
/// <param name="Reason">Reason of the violation.</param>
/// <param name="TimeLimit">Maximum time duration a computation can take.</param>
/// <param name="Duration">Elapsed time since the start of the computation.</param>
public record TimeLimitViolation(string Reason, TimeSpan TimeLimit, TimeSpan Duration)
    : ConstraintViolation(Reason);

/// <summary>
/// Multiple constraint violation.
/// </summary>
public record MultiViolation : ConstraintViolation
{
    /// <summary>
    /// Details of the violations.
    /// </summary>
    public IReadOnlyList<ConstraintViolation> Violations { get; }

    /// <summary>
    /// Multiple constraint violation.
    /// </summary>
    /// <param name="reason">Reason of the violation.</param>
    /// <param name="violations">Violations to ensure.</param>
    public MultiViolation(string reason, IEnumerable<ConstraintViolation> violations)
        : base(reason)
    {
        Violations = violations.ToList().AsReadOnly();
    }
}
