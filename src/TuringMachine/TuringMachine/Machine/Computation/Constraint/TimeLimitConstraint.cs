using System;

namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Ensures that computation can not take more time than the limit.
/// </summary>
public class TimeLimitConstraint : IComputationConstraint<IReadOnlyComputationState>
{
    private readonly TimeSpan timeLimit;

    /// <summary>
    /// Initializes a new instance of <see cref="TimeLimitConstraint"/> class with the specified time limit.
    /// </summary>
    /// <param name="timeLimit">Maximum time duration a computation can take.</param>
    /// <exception cref="ArgumentOutOfRangeException">Time limit is less than or equal to <see cref="TimeSpan.Zero"/>.</exception>
    public TimeLimitConstraint(TimeSpan timeLimit)
    {
        if (timeLimit <= TimeSpan.Zero)
        {
            string message = $"Timeout must be greater than {nameof(TimeSpan)}.{nameof(TimeSpan.Zero)}.";
            throw new ArgumentOutOfRangeException(nameof(timeLimit), timeLimit, message);
        }

        this.timeLimit = timeLimit;
    }

    public ConstraintViolation? Enforce(IReadOnlyComputationState computationState)
    {
        return computationState.Duration > timeLimit
            ? new TimeLimitViolation($"Computation takes longer than {timeLimit}.", timeLimit, computationState.Duration)
            : null;
    }
}
