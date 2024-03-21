using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents the domain of a multi-tape machine transition.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class TransitionDomain<TState, TSymbol> : IEquatable<TransitionDomain<TState, TSymbol>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransitionDomain{TState, TSymbol}"/> class with the specified state and tape symbols.
    /// </summary>
    /// <param name="state">State of the domain.</param>
    /// <param name="tapeSymbols">Symbols per tape of the domain.</param>
    /// <exception cref="ArgumentException">Empty tape symbol collection provided.</exception>
    public TransitionDomain(State<TState> state, IEnumerable<Symbol<TSymbol>> tapeSymbols)
    {
        if (!tapeSymbols.Any())
        {
            throw new ArgumentException("Collection must contain at least one tape symbol.", nameof(tapeSymbols));
        }

        State = state;
        TapeSymbols = tapeSymbols.ToList().AsReadOnly();
    }

    /// <summary>
    /// State of the domain.
    /// </summary>
    public State<TState> State { get; }

    /// <summary>
    /// Symbols per tape of the domain.
    /// </summary>
    public IReadOnlyList<Symbol<TSymbol>> TapeSymbols { get; }

	public bool Equals(TransitionDomain<TState, TSymbol>? other)
	{
		if (other is null)
		{
			return false;
		}

		if (!State.Equals(other.State) || TapeSymbols.Count != other.TapeSymbols.Count)
		{
			return false;
		}

		for (int i = 0; i < TapeSymbols.Count; ++i)
		{
			if (!TapeSymbols[i].Equals(other.TapeSymbols[i]))
			{
				return false;
			}
		}

		return true;
	}

	public override bool Equals(object? obj)
    {
        return obj is TransitionDomain<TState, TSymbol> other ? Equals(other) : false;
    }

    /// <summary>
    /// Determines whether two specified domains have the same value.
    /// </summary>
    /// <param name="left">The first symbol to compare.</param>
    /// <param name="right">The second domain to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(TransitionDomain<TState, TSymbol>? left, TransitionDomain<TState, TSymbol>? right)
    {
        return EqualityComparer<TransitionDomain<TState, TSymbol>>.Default.Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified domains have different value.
    /// </summary>
    /// <param name="left">The first domains to compare.</param>
    /// <param name="right">The second domains to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(TransitionDomain<TState, TSymbol>? left, TransitionDomain<TState, TSymbol>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hashcode for this instance.
    /// </summary>
    /// <returns><see cref="int"/> hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 0;

            for (int i = 0; i < TapeSymbols.Count; ++i)
            {
                hash += (i + 1) * TapeSymbols[i].GetHashCode();
            }

            return hash * State.GetHashCode();
        }
    }
}
