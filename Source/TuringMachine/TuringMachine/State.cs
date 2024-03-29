﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine;

/// <summary>
/// Represents a Turing machine state.
/// </summary>
/// <typeparam name="T">Type of the state.</typeparam>
public class State<T> : IEquatable<State<T>>
{
    private const int NullValueHashCode = 7919;
    private const int InitialHashCode = 100003;
    private const int AcceptHashCode = 500009;
    private const int RejectHashCode = 900007;

	static State()
	{
		Initial = new State<T>(default!);
		Accept = new State<T>(default!);
		Reject = new State<T>(default!);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="State{T}"/> class with the given value.
	/// </summary>
	/// <param name="value">The value that represents a state.</param>
	public State(T value) => Value = value;

	/// <summary>
	/// Initial state of the machine, from where the first symbol is read.
	/// </summary>
	public static State<T> Initial { get; }

    /// <summary>
    /// Accept state of the machine, when computation terminated by accepting the input.
    /// </summary>
    public static State<T> Accept { get; }

    /// <summary>
    /// Reject state of the machine, when there is no valid transition from the current state of the machine.
    /// </summary>
    public static State<T> Reject { get; }

    /// <summary>
    /// Gets the value that represents the state.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets whether the current instance is a finished computation state.
    /// </summary>
    public bool Finish => this == Accept || this == Reject;

    public bool Equals(State<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (SameSpecialStates(other, this))
        {
            return true;
        }

        return !AnyOfThemSpecialState(other, this) && EqualityComparer<T>.Default.Equals(other.Value, Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is State<T> other ? Equals(other) : false;
    }

    /// <summary>
    /// Returns the hash code for this state.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return this switch
        {
            _ when ReferenceEquals(this, Initial) => InitialHashCode,
            _ when ReferenceEquals(this, Accept) => AcceptHashCode,
            _ when ReferenceEquals(this, Reject) => RejectHashCode,
            _ => Value?.GetHashCode() ?? NullValueHashCode
        };
    }

    /// <summary>
    /// Determines whether two specified states have the same value.
    /// </summary>
    /// <param name="left">The first state to compare.</param>
    /// <param name="right">The second state to compare.</param>
    /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
    public static bool operator ==(State<T>? left, State<T>? right)
    {
        return EqualityComparer<State<T>>.Default.Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified states have different value.
    /// </summary>
    /// <param name="left">The first state to compare.</param>
    /// <param name="right">The second state to compare.</param>
    /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
    public static bool operator !=(State<T>? left, State<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts the value of this instance to a <see cref="string"/>.
    /// </summary>
    /// <returns>
    /// <see cref="string"/> whose value is the same as the string representation of <see cref="Value"/>. If it's a special state then
    /// it returns the special state's name.
    /// </returns>
    public override string ToString()
    {
        return this switch
        {
            _ when ReferenceEquals(this, Initial) => nameof(Initial),
            _ when ReferenceEquals(this, Accept) => nameof(Accept),
            _ when ReferenceEquals(this, Reject) => nameof(Reject),
            _ => $"{Value}"
        };
    }

    private bool SameSpecialStates(object first, object second)
    {
        return GetSpecialStates().Any(special => ReferenceEquals(first, special) && ReferenceEquals(second, special));
    }

    private bool AnyOfThemSpecialState(object first, object second)
    {
        return GetSpecialStates().Any(special => ReferenceEquals(first, special) || ReferenceEquals(second, special));
    }

    private IEnumerable<State<T>> GetSpecialStates()
    {
        yield return Initial;
        yield return Accept;
        yield return Reject;
    }
}
