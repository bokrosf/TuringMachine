using TuringMachine.Machine;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents the range of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    /// <param name="State">State of the range.</param>
    /// <param name="Symbol">Symbol of the range.</param>
    /// <param name="HeadDirection">Movement direction of tape's head.</param>
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
}
