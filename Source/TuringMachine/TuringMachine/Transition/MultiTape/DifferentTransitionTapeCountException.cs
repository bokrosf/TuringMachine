using System;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Exception that is thrown when a multi-tape transition has different tape count than other transitions.
/// </summary>
public class DifferentTransitionTapeCountException : InvalidTransitionCollectionException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DifferentTransitionTapeCountException"/> class.
    /// </summary>
    public DifferentTransitionTapeCountException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DifferentTransitionTapeCountException"/> class with a specified error message.
    /// <param name="message">The message that describes the error.</param>
    public DifferentTransitionTapeCountException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DifferentTransitionTapeCountException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DifferentTransitionTapeCountException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}