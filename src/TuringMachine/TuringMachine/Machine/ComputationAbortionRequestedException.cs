using System;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Exception that is thrown when abortion of a computation is requested.
    /// </summary>
    public class ComputationAbortionRequestedException : ComputationAbortedException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortionRequestedException"/> class.
        /// </summary>
        public ComputationAbortionRequestedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortionRequestedException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public ComputationAbortionRequestedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationAbortionRequestedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ComputationAbortionRequestedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
