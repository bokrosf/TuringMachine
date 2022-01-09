using System;
using System.Collections.Generic;
using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine.Computation
{
    /// <summary>
    /// Provides data for the event that is raised when a computation has been aborted.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class ComputationAbortedEventArgs<TState, TSymbol> : ComputationTerminatedEventArgs<TState, TSymbol>
    {
        /// <summary>
        /// The exception that caused the abortion.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// The constraint violation that caused the abortion.
        /// </summary>
        public ConstraintViolation? ConstraintViolation { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortedEventArgs{TState, TSymbol}"/> class with the specified computation state, 
        /// the resulting symbols of the computation and the cause of abortion.
        /// </summary>
        /// <param name="computationState">State of the computation.</param>
        /// <param name="result">Symbols from the machine's tape after the computation has terminated.</param>
        /// <param name="exception">The exception that caused the abortion.</param>
        /// <param name="constraintViolation"></param>
        public ComputationAbortedEventArgs(
            IReadOnlyComputationState<TState, TSymbol> computationState, 
            IEnumerable<Symbol<TSymbol>> result,
            Exception? exception,
            ConstraintViolation? constraintViolation)
            : base(computationState, result)
        {
            Exception = exception;
            ConstraintViolation = constraintViolation;
        }
    }
}
