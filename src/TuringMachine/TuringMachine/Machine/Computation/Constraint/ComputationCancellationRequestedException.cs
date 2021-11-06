using System;

namespace TuringMachine.Machine.Computation.Constraint
{
    /// <summary>
    /// Exception that is thrown when cancellation of a computation is requested.
    /// </summary>
    public class ComputationCancellationRequestedException : ComputationAbortedException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ComputationCancellationRequestedException"/> class.
        /// </summary>
        public ComputationCancellationRequestedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationCancellationRequestedException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public ComputationCancellationRequestedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationCancellationRequestedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ComputationCancellationRequestedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
