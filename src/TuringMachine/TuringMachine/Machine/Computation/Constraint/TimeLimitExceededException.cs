using System;
using TuringMachine.Extensions.ExceptionCustomizer;

namespace TuringMachine.Machine.Computation.Constraint
{
    /// <summary>
    /// Exception that is thrown when a computation should have not taken longer then the specified time duration.
    /// </summary>
    public class TimeLimitExceededException : ComputationAbortedException
    {
        /// <summary>
        /// Maximum time duration the computation should have taken.
        /// </summary>
        public TimeSpan? TimeLimit { get; }

        /// <summary>
        /// Elapsed time since the start of the computation.
        /// </summary>
        public TimeSpan? Duration { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitExceededException"/> class.
        /// </summary>
        public TimeLimitExceededException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitExceededException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TimeLimitExceededException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitExceededException"/> class with a specified error message, time limit and duration.
        /// <param name="message">The message that describes the error.</param>
        /// <param name="timeLimit">Maximum time duration the computation should have taken.</param>
        /// <param name="duration">Elapsed time since the start of the computation.</param>
        public TimeLimitExceededException(string? message, TimeSpan? timeLimit, TimeSpan? duration)
            : base(message)
        {
            TimeLimit = timeLimit;
            Duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TimeLimitExceededException"/> class with a specified error message,
        /// a reference to the inner exception that is the cause of this exception, time limit and duration.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="timeLimit">Maximum time duration the computation should have taken.</param>
        /// <param name="duration">Elapsed time since the start of the computation.</param>
        public TimeLimitExceededException(string? message, Exception? innerException, TimeSpan? timeLimit, TimeSpan? duration)
            : base(message, innerException)
        {
            TimeLimit = timeLimit;
            Duration = duration;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.CustomizeToString(
                $"{nameof(TimeLimit)}: {TimeLimit}", 
                $"{nameof(Duration)}: {Duration}");
        }
    }
}
