using System;

namespace TuringMachine.Machine.Computation.Constraint
{
    /// <summary>
    /// Ensures that computation can not take more step than the limit.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class StepLimitConstraint<TState, TSymbol> : IComputationConstraint<TState, TSymbol>
    {
        private readonly int stepLimit;

        /// <summary>
        /// Initializes a new instance of <see cref="StepLimitConstraint{TState, TSymbol}"/> class with the specified step limit.
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
        
        /// <inheritdoc/>
        /// <exception cref="StepLimitExceededException">Computation takes more step than the limit.</exception>
        public void Enforce(IReadOnlyComputationState<TState, TSymbol> computationState)
        {
            if (computationState.StepCount > stepLimit)
            {
                throw new StepLimitExceededException($"Computation can not take more than {stepLimit} steps.", stepLimit);
            }
        }
    }
}
