using System;

namespace TuringMachine.Machine.Computation.Constraint;

/// <summary>
/// Ensures that computation can not take more step than the limit.
/// </summary>
public class StepLimitConstraint : IComputationConstraint<IReadOnlyComputationState>
{
    private readonly int stepLimit;

    /// <summary>
    /// Initializes a new instance of <see cref="StepLimitConstraint"/> class with the specified step limit.
    /// </summary>
    /// <param name="stepLimit">Maximum number of steps a computation can take.</param>
    /// <exception cref="ArgumentOutOfRangeException">Step limit is less than 1.</exception>
    public StepLimitConstraint(int stepLimit)
    {
        if (stepLimit < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(stepLimit), stepLimit, $"Step limit must be greater than 0.");
        }

        this.stepLimit = stepLimit;
    }

    public ConstraintViolation? Enforce(IReadOnlyComputationState computationState)
    {
        return computationState.StepCount > stepLimit
            ? new StepLimitViolation($"Computation can not take more than {stepLimit} steps.", stepLimit)
            : null;
    }
}
