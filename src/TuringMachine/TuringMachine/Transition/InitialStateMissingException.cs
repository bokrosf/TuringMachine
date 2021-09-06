using System;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Exception that is thrown when the initial state is missing from transitions.
    /// </summary>
    public class InitialStateMissingException : InvalidTransitionCollectionException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InitialStateMissingException"/> class.
        /// </summary>
        public InitialStateMissingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InitialStateMissingException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public InitialStateMissingException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InitialStateMissingException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InitialStateMissingException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
