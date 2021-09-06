namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents the domain of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public record TransitionDomain<TState, TValue>(State<TState> State, Symbol<TValue> Symbol)
    {
        /// <summary>
        /// Converts the given touple of state and value into a <see cref="TransitionDomain{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TValue>((TState State, TValue Value) domain)
        {
            return new TransitionDomain<TState, TValue>(
                new State<TState>(domain.State),
                new Symbol<TValue>(domain.Value));
        }

        /// <summary>
        /// Converts the given touple of state and symbol into a <see cref="TransitionDomain{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TValue>((State<TState> State, Symbol<TValue> Symbol) domain)
        {
            return new TransitionDomain<TState, TValue>(domain.State, domain.Symbol);
        }

        /// <summary>
        /// Converts the given touple of state and value into a <see cref="TransitionDomain{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TValue>((TState State, Symbol<TValue> Symbol) domain)
        {
            return new TransitionDomain<TState, TValue>(
                new State<TState>(domain.State),
                domain.Symbol);
        }

        /// <summary>
        /// Converts the given touple of state and value into a <see cref="TransitionDomain{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        public static implicit operator TransitionDomain<TState, TValue>((State<TState> State, TValue Symbol) domain)
        {
            return new TransitionDomain<TState, TValue>(
                domain.State,
                new Symbol<TValue>(domain.Symbol));
        }
    }

    /// <summary>
    /// Represents the range of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public record TransitionRange<TState, TValue>(State<TState> State, Symbol<TValue> Symbol, TapeHeadDirection HeadDirection)
    {
        /// <summary>
        /// Converts the given touple of state, value and head direction into a <see cref="TransitionRange{TState, TValue}{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TValue>((TState State, TValue Value, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TValue>(
                new State<TState>(range.State),
                new Symbol<TValue>(range.Value),
                range.HeadDirection);
        }

        /// <summary>
        /// Converts the given touple of state, symbol and head direction into a <see cref="TransitionRange{TState, TValue}{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TValue>((State<TState> State, Symbol<TValue> Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TValue>(range.State, range.Symbol, range.HeadDirection);
        }

        /// <summary>
        /// Converts the given touple of state, symbol and head direction into a <see cref="TransitionRange{TState, TValue}{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TValue>((TState State, Symbol<TValue> Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TValue>(
                new State<TState>(range.State), 
                range.Symbol, 
                range.HeadDirection);
        }

        /// <summary>
        /// Converts the given touple of state, symbol and head direction into a <see cref="TransitionRange{TState, TValue}{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="range">Range of a machine transition.</param>
        public static implicit operator TransitionRange<TState, TValue>((State<TState> State, TValue Symbol, TapeHeadDirection HeadDirection) range)
        {
            return new TransitionRange<TState, TValue>(
                range.State, 
                new Symbol<TValue>(range.Symbol), 
                range.HeadDirection);
        }
    }

    /// <summary>
    /// Represents a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public record Transition<TState, TValue>(TransitionDomain<TState, TValue> Domain, TransitionRange<TState, TValue> Range)
    {
        /// <summary>
        /// Converts the given touple of a transition's domain and range into a <see cref="Transition{TState, TValue}{TState, TValue}"/> instance.
        /// </summary>
        /// <param name="transition">Machine transition.</param>
        public static implicit operator Transition<TState, TValue>(
            (TransitionDomain<TState, TValue> Domain, TransitionRange<TState, TValue> Range) transition)
        {
            return new Transition<TState, TValue>(transition.Domain, transition.Range);
        }
    }
}
