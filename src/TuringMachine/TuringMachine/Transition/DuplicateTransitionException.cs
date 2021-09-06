using System;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Exception that is thrown when a machine transition duplication detected.
    /// </summary>
    public class DuplicateTransitionException : InvalidTransitionCollectionException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DuplicateTransitionException"/> class.
        /// </summary>
        public DuplicateTransitionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DuplicateTransitionException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public DuplicateTransitionException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DuplicateTransitionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DuplicateTransitionException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
