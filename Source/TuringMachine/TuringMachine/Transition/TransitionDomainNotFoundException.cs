using System;

namespace TuringMachine.Transition;

/// <summary>
/// Exception that is thrown when a machine transition domain is not found.
/// </summary>
public class TransitionDomainNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransitionDomainNotFoundException"/> class.
    /// </summary>
    public TransitionDomainNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransitionDomainNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public TransitionDomainNotFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransitionDomainNotFoundException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public TransitionDomainNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
