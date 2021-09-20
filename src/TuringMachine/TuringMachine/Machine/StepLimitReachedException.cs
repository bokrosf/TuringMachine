﻿using System;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Exception that is thrown when a computation has been aborted because it has not finished in the given number of steps.
    /// </summary>
    public class StepLimitReachedException : ComputationAbortedException
    {
        /// <summary>
        /// The maximum number of steps the computation should have finished.
        /// </summary>
        public int StepLimit { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="StepLimitReachedException"/> class with the specified step limit.
        /// </summary>
        /// <param name="stepLimit">The maximum number of steps the computation should have finished.</param>
        public StepLimitReachedException(int stepLimit)
        {
            StepLimit = stepLimit;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StepLimitReachedException"/> class with a specified error message and step limit.
        /// <param name="message">The message that describes the error.</param>
        /// <param name="stepLimit">The maximum number of steps the computation should have finished.</param>
        public StepLimitReachedException(string? message, int stepLimit)
            : base(message)
        {
            StepLimit = stepLimit;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StepLimitReachedException"/> class with a specified error message,
        /// a reference to the inner exception that is the cause of this exception and step limit.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="stepLimit">The maximum number of steps the computation should have finished.</param>
        public StepLimitReachedException(string? message, Exception? innerException, int stepLimit)
            : base(message, innerException)
        {
            StepLimit = stepLimit;
        }
    }
}
