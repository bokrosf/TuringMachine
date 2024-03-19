﻿using System;

namespace TuringMachine.Transition;

/// <summary>
/// Exception that is thrown when a non deterministic machine transition detected.
/// </summary>
public class NonDeterministicTransitionException : InvalidTransitionCollectionException
{
    /// <summary>
    /// Initializes a new instance of <see cref="NonDeterministicTransitionException"/> class.
    /// </summary>
    public NonDeterministicTransitionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NonDeterministicTransitionException"/> class with a specified error message.
    /// <param name="message">The message that describes the error.</param>
    public NonDeterministicTransitionException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NonDeterministicTransitionException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NonDeterministicTransitionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
