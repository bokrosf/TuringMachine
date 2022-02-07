using System;

namespace TuringMachine.Transition;

/// <summary>
/// Exception that is thrown when a machine transition collection contains an invalid transition.
/// </summary>
public class InvalidTransitionCollectionException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="InvalidTransitionCollectionException"/> class.
    /// </summary>
    public InvalidTransitionCollectionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="InvalidTransitionCollectionException"/> class with a specified error message.
    /// <param name="message">The message that describes the error.</param>
    public InvalidTransitionCollectionException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="InvalidTransitionCollectionException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidTransitionCollectionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
