using TuringMachine.Machine;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents tape changes that occur during machine transition.
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <param name="Domain">Current symbol pointed by the tape's head.</param>
/// <param name="Range">The current symbol pointed by the tape's head is overwritten by this symbol.</param>
/// <param name="TapeHeadDirection">Direction of the tape head's movement.</param>
public record TapeTransition<TSymbol>(Symbol<TSymbol> Domain, Symbol<TSymbol> Range, TapeHeadDirection TapeHeadDirection)
{
    /// <summary>
    /// Converts the given tuple of domain, range and tape head direction into a <see cref="TapeTransition{TSymbol}"/> instance.
    /// </summary>
    /// <param name="transition">Tape transition of a machine.</param>
    public static implicit operator TapeTransition<TSymbol>((TSymbol Domain, TSymbol Range, TapeHeadDirection TapeHeadDirection) transition)
    {
        return new TapeTransition<TSymbol>(
            new Symbol<TSymbol>(transition.Domain),
            new Symbol<TSymbol>(transition.Range),
            transition.TapeHeadDirection);
    }

    /// <summary>
    /// Converts the given tuple of domain, range and tape head direction into a <see cref="TapeTransition{TSymbol}"/> instance.
    /// </summary>
    /// <param name="transition">Tape transition of a machine.</param>
    public static implicit operator TapeTransition<TSymbol>((Symbol<TSymbol> Domain, Symbol<TSymbol> Range, TapeHeadDirection TapeHeadDirection) transition)
    {
        return new TapeTransition<TSymbol>(
            transition.Domain, 
            transition.Range, 
            transition.TapeHeadDirection);
    }

    /// <summary>
    /// Converts the given tuple of domain, range and tape head direction into a <see cref="TapeTransition{TSymbol}"/> instance.
    /// </summary>
    /// <param name="transition">Tape transition of a machine.</param>
    public static implicit operator TapeTransition<TSymbol>((Symbol<TSymbol> Domain, TSymbol Range, TapeHeadDirection TapeHeadDirection) transition)
    {
        return new TapeTransition<TSymbol>(
            transition.Domain,
            new Symbol<TSymbol>(transition.Range),
            transition.TapeHeadDirection);
    }

    /// <summary>
    /// Converts the given tuple of domain, range and tape head direction into a <see cref="TapeTransition{TSymbol}"/> instance.
    /// </summary>
    /// <param name="transition">Tape transition of a machine.</param>
    public static implicit operator TapeTransition<TSymbol>((TSymbol Domain, Symbol<TSymbol> Range, TapeHeadDirection TapeHeadDirection) transition)
    {
        return new TapeTransition<TSymbol>(
            new Symbol<TSymbol>(transition.Domain),
            transition.Range,
            transition.TapeHeadDirection);
    }
}
