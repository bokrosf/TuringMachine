using System;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Exception that is thrown when accept state is missing from transitions.
    /// </summary>
    public class AcceptStateMissingException : InvalidTransitionCollectionException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AcceptStateMissingException"/> class.
        /// </summary>
        public AcceptStateMissingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AcceptStateMissingException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public AcceptStateMissingException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AcceptStateMissingException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AcceptStateMissingException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
