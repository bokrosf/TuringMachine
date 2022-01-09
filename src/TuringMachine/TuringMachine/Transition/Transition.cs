using TuringMachine.Machine;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents the domain of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public record TransitionDomain<TState, TSymbol>(State<TState> State, Symbol<TSymbol> Symbol)
    {
        /// <summary>
        /// Converts the given tuple of state and symbol into a <see cref="TransitionDomain{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TSymbol>((TState State, TSymbol Symbol) domain)
        {
            return new TransitionDomain<TState, TSymbol>(
                new State<TState>(domain.State),
                new Symbol<TSymbol>(domain.Symbol));
        }

        /// <summary>
        /// Converts the given tuple of state and symbol into a <see cref="TransitionDomain{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TSymbol>((State<TState> State, Symbol<TSymbol> Symbol) domain)
        {
            return new TransitionDomain<TState, TSymbol>(domain.State, domain.Symbol);
        }

        /// <summary>
        /// Converts the given tuple of state and symbol into a <see cref="TransitionDomain{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TSymbol>((TState State, Symbol<TSymbol> Symbol) domain)
        {
            return new TransitionDomain<TState, TSymbol>(
                new State<TState>(domain.State),
                domain.Symbol);
        }

        /// <summary>
        /// Converts the given tuple of state and symbol into a <see cref="TransitionDomain{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TSymbol>((State<TState> State, TSymbol Symbol) domain)
        {
            return new TransitionDomain<TState, TSymbol>(
                domain.State,
                new Symbol<TSymbol>(domain.Symbol));
        }
    }

    /// <summary>
    /// Represents the range of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public record TransitionRange<TState, TSymbol>(State<TState> State, Symbol<TSymbol> Symbol, TapeHeadDirection HeadDirection)
    {
        /// <summary>
        /// Converts the given tuple of state, symbol and head direction into a <see cref="TransitionRange{TState, TSymbol}{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TSymbol>((TState State, TSymbol Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TSymbol>(
                new State<TState>(range.State),
                new Symbol<TSymbol>(range.Symbol),
                range.HeadDirection);
        }

        /// <summary>
        /// Converts the given tuple of state, symbol and head direction into a <see cref="TransitionRange{TState, TSymbol}{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TSymbol>((State<TState> State, Symbol<TSymbol> Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TSymbol>(range.State, range.Symbol, range.HeadDirection);
        }

        /// <summary>
        /// Converts the given tuple of state, symbol and head direction into a <see cref="TransitionRange{TState, TSymbol}{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TSymbol>((TState State, Symbol<TSymbol> Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TSymbol>(
                new State<TState>(range.State), 
                range.Symbol, 
                range.HeadDirection);
        }

        /// <summary>
        /// Converts the given tuple of state, symbol and head direction into a <see cref="TransitionRange{TState, TSymbol}{TState, TSymbol}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TSymbol>((State<TState> State, TSymbol Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TSymbol>(
                range.State, 
                new Symbol<TSymbol>(range.Symbol), 
                range.HeadDirection);
        }
    }

    /// <summary>
    /// Represents a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
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
}
