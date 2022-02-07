﻿using System;

namespace TuringMachine.Transition;

/// <summary>
/// Exception that is thrown when a machine state is missing.
/// </summary>
public class MissingStateException : InvalidTransitionCollectionException
{
    /// <summary>
    /// Initializes a new instance of <see cref="MissingStateException"/> class.
    /// </summary>
    public MissingStateException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="MissingStateException"/> class with a specified error message.
    /// <param name="message">The message that describes the error.</param>
    public MissingStateException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="MissingStateException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public MissingStateException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}