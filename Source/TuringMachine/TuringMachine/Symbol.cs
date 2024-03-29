﻿using System;
using System.Collections.Generic;

namespace TuringMachine;

/// <summary>
/// Represents a Turing machine tape symbol.
/// </summary>
/// <typeparam name="T">Typeof of the symbolised data.</typeparam>
public sealed class Symbol<T> : IEquatable<Symbol<T>>
{
    private const int NullValueHashCode = 7723;

	static Symbol() => Blank = new Symbol<T>(default!);

	/// <summary>
	/// Initializes a new instance of the <see cref="Symbol{T}"/> class with the given value.
	/// </summary>
	/// <param name="value">The value to be stored on the tape.</param>
	public Symbol(T value) => Value = value;

	/// <summary>
	/// Gets the blank symbol.
	/// </summary>
	public static Symbol<T> Blank { get; }

    /// <summary>
    /// Gets the symbolised value.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Converts the value of this instance to a <see cref="string"/>.
    /// </summary>
    /// <returns>
    /// <see cref="string"/> whose value is the same as the string representation of <see cref="Value"/>. If it's the <see cref="Blank"/>
    /// symbol then it returns "&lt;BLANK&gt;".
    /// </returns>
    public override string ToString()
    {
        return ReferenceEquals(this, Blank) ? "<BLANK>" : $"<{Value}>";
    }

    public bool Equals(Symbol<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        bool otherIsBlank = ReferenceEquals(other, Blank);
        bool thisIsBlank = ReferenceEquals(this, Blank);

        if (otherIsBlank && thisIsBlank)
        {
            return true;
        }

        return !otherIsBlank && !thisIsBlank && EqualityComparer<T>.Default.Equals(other.Value, Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Symbol<T> other ? Equals(other) : false;
    }

    /// <summary>
    /// Determines whether two specified symbols have the same value.
    /// </summary>
    /// <param name="left">The first symbol to compare.</param>
    /// <param name="right">The second symbol to compare.</param>
    /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
    public static bool operator ==(Symbol<T>? left, Symbol<T>? right)
    {
        return EqualityComparer<Symbol<T>>.Default.Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified symbols have different value.
    /// </summary>
    /// <param name="left">The first symbol to compare.</param>
    /// <param name="right">The second symbol to compare.</param>
    /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
    public static bool operator !=(Symbol<T>? left, Symbol<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hash code for this symbol.
    /// </summary>
    public override int GetHashCode()
    {
        if (ReferenceEquals(Blank, this))
        {
            return base.GetHashCode();
        }

        return Value?.GetHashCode() ?? NullValueHashCode;
    }
}
