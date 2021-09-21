﻿using System;

namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Ensures that computation can not take more time than the limit.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class TimeLimitConstraint<TState, TSymbol> : ComputationConstraint<TState, TSymbol>
    {
        private readonly TimeSpan timeLimit;

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitConstraint{TState, TSymbol}"/> class with the specified time limit.
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
        
        /// <inheritdoc/>
        /// <exception cref="TimeLimitConstraint{TState, TSymbol}">Computation takes longer than the time limit.</exception>
        public override void Enforce(ReadOnlyComputationState<TState, TSymbol> computationState)
        {
            if (computationState.ElapsedTime > timeLimit && !IsComputationFinished(computationState))
            {
                throw new TimeLimitReachedException($"Computation takes longer than {timeLimit}.", timeLimit);
            }
        }
    }
}
