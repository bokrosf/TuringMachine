using System;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Exception that is thrown when a computation has been aborted.
    /// </summary>
    public class ComputationAbortedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortedException"/> class.
        /// </summary>
        public ComputationAbortedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortedException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public ComputationAbortedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ComputationAbortedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
