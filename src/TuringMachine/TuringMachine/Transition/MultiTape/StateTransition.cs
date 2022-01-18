namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents a state transition of a machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <param name="Domain">Current state of the machine.</param>
/// <param name="Range">State that the machine transits to.</param>
public record StateTransition<TState>(State<TState> Domain, State<TState> Range)
{
    /// <summary>
    /// Converts the given tuple of domain and range states into a <see cref="StateTransition{TState}"/> instance.
    /// </summary>
    /// <param name="transition">State transition of a machine.</param>
    public static implicit operator StateTransition<TState>((TState Domain, TState Range) transition)
    {
        return new StateTransition<TState>(
            new State<TState>(transition.Domain),
            new State<TState>(transition.Range));
    }

    /// <summary>
    /// Converts the given tuple of domain and range states into a <see cref="StateTransition{TState}"/> instance.
    /// </summary>
    /// <param name="transition">State transition of a machine.</param>
    public static implicit operator StateTransition<TState>((State<TState> Domain, State<TState> Range) transition)
    {
        return new StateTransition<TState>(transition.Domain, transition.Range);
    }

    /// <summary>
    /// Converts the given tuple of domain and range states into a <see cref="StateTransition{TState}"/> instance.
    /// </summary>
    /// <param name="transition">State transition of a machine.</param>
    public static implicit operator StateTransition<TState>((State<TState> Domain, TState Range) transition)
    {
        return new StateTransition<TState>(
            transition.Domain,
            new State<TState>(transition.Range));
    }

    /// <summary>
    /// Converts the given tuple of domain and range states into a <see cref="StateTransition{TState}"/> instance.
    /// </summary>
    /// <param name="transition">State transition of a machine.</param>
    public static implicit operator StateTransition<TState>((TState Domain, State<TState> Range) transition)
    {
        return new StateTransition<TState>(
            new State<TState>(transition.Domain), 
            transition.Range);
    }
}
