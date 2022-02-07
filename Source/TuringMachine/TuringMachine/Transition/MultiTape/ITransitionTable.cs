namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents the mapping of transition domains and ranges that can be used by a multi-tape machine for transitioning 
/// from one configuration to the next.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface ITransitionTable<TState, TSymbol>
{
    /// <summary>
    /// Gets the number of tapes.
    /// </summary>
    int TapeCount { get; }
    
    /// <summary>
    ///  Gets the range that belongs to the given transition domain.
    /// </summary>
    /// <param name="domain">Domain of a transition</param>
    /// <returns><see cref="TransitionRange{TState, TSymbol}"/> that belongs to the given transition domain.</returns>
    internal TransitionRange<TState, TSymbol> this[TransitionDomain<TState, TSymbol> domain] { get; }
}
