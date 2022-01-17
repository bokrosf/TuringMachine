namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents the domain of a machine transition.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    /// <param name="State">State of the domain.</param>
    /// <param name="Symbol">Symbol of the domain.</param>
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
}
