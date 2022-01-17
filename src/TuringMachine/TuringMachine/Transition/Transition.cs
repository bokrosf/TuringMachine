namespace TuringMachine.Transition;

/// <summary>
/// Represents a machine transition.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <param name="Domain">Domain of the transition.</param>
/// <param name="Range">Range of the transition.</param>
public record Transition<TState, TSymbol>(TransitionDomain<TState, TSymbol> Domain, TransitionRange<TState, TSymbol> Range)
{
    /// <summary>
    /// Converts the given tuple of a transition's domain and range into a <see cref="Transition{TState, TSymbol}{TState, TSymbol}"/> instance.
    /// </summary>
    /// <param name="transition">Machine transition.</param>
    public static implicit operator Transition<TState, TSymbol>(
        (TransitionDomain<TState, TSymbol> Domain, TransitionRange<TState, TSymbol> Range) transition)
    {
        return new Transition<TState, TSymbol>(transition.Domain, transition.Range);
    }
}
