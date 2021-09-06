using System;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Exception that is thrown when no machine transition has been provided.
    /// </summary>
    public class NoTransitionProvidedException : InvalidTransitionCollectionException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NoTransitionProvidedException"/> class.
        /// </summary>
        public NoTransitionProvidedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoTransitionProvidedException"/> class with a specified error message.
        /// <param name="message">The message that describes the error.</param>
        public NoTransitionProvidedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoTransitionProvidedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NoTransitionProvidedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
