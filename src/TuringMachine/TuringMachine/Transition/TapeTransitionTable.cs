using System.Collections.Generic;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents machine transitions of a single tape.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    internal class TapeTransitionTable<TState, TValue>
    {
        private readonly Dictionary<TransitionDomain<TState, TValue>, TransitionRange<TState, TValue>> transitions;

        /// <summary>
        /// Initializes a new instance of <see cref="TapeTransitionTable{TState, TValue}"/> class.
        /// </summary>
        public TapeTransitionTable()
        {
            transitions = new Dictionary<TransitionDomain<TState, TValue>, TransitionRange<TState, TValue>>();
        }

        /// <summary>
        /// Gets or sets the value associated with the specified transition domain.
        /// </summary>
        /// <param name="domain">Domain of a machine transition.</param>
        /// <returns><see cref="TransitionRange{TState, TValue}"/> that belongs to the given transition domain.</returns>
        /// <exception cref="TransitionDomainNotFoundException"><paramref name="domain"/> is not found in the table.</exception>
        public TransitionRange<TState, TValue> this[TransitionDomain<TState, TValue> domain]
        {
            get
            {
                if (transitions.TryGetValue(domain, out var range))
                {
                    return range;
                }

                throw new TransitionDomainNotFoundException($"{nameof(domain)}={domain}");
            }

            set => transitions[domain] = value;
        }
    }
}
