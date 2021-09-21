using System;

namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Exception that is thrown when a computation should have not taken longer then the specified time duration.
    /// </summary>
    public class TimeLimitReachedException : ComputationAbortedException
    {
        /// <summary>
        /// Maximum time duration the computation should have taken.
        /// </summary>
        public TimeSpan TimeLimit { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitReachedException"/> class with the specified time limit.
        /// </summary>
        /// <param name="timeLimit">Maximum time duration the computation should have taken.</param>
        public TimeLimitReachedException(TimeSpan timeLimit)
        {
            TimeLimit = timeLimit;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitReachedException"/> class with a specified error message and time limit.
        /// <param name="message">The message that describes the error.</param>
        /// <param name="timeLimit">Maximum time duration the computation should have taken.</param>
        public TimeLimitReachedException(string? message, TimeSpan timeLimit)
            : base(message)
        {
            TimeLimit = timeLimit;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitReachedException"/> class with a specified error message,
        /// a reference to the inner exception that is the cause of this exception and time limit.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="timeLimit">Maximum time duration the computation should have taken.</param>
        public TimeLimitReachedException(string? message, Exception? innerException, TimeSpan timeLimit)
            : base(message, innerException)
        {
            TimeLimit = timeLimit;
        }
    }
}
