using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Represents the mapping of transition domains and ranges that can be used by a machine for transitioning one configuration to the next.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public class TransitionTable<TState, TValue>
    {
        private readonly ReadOnlyDictionary<TransitionDomain<TState, TValue>, TransitionRange<TState, TValue>> transitions;

        /// <summary>
        /// Gets the range that belongs to the given transition domain.
        /// </summary>
        /// <param name="domain">Domain of a transitions.</param>
        /// <returns><see cref="TransitionRange{TState, TValue}"/> that belongs to the given transition domain.</returns>
        internal TransitionRange<TState, TValue> this[TransitionDomain<TState, TValue> domain]
        {
            get
            {
                return transitions.TryGetValue(domain, out var range)
                    ? range
                    : throw new TransitionDomainNotFoundException($"Not found Domain={domain}.");
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TransitionTable{TState, TValue}"/> class with the given collection of transitions.
        /// </summary>
        /// <param name="transitions">Transitions.</param>
        /// <exception cref="NoTransitionProvidedException">Thrown when no machine transition has been provided.</exception>
        /// <exception cref="DuplicateTransitionException">Thrown when the collection contains a duplicate transition.</exception>
        /// <exception cref="NonDeterministicTransitionException">Thrown when the collection contains a transition domain more than once.</exception>
        /// <exception cref="InvalidStateInTransitionException">Thrown when the collection contains a transition with an invalid state.</exception>
        public TransitionTable(IEnumerable<Transition<TState, TValue>> transitions)
        {
            new TransitionCollectionValidator<TState, TValue>().Validate(transitions);
            var mapping = transitions.ToDictionary(t => t.Domain, t => t.Range);
            this.transitions = new ReadOnlyDictionary<TransitionDomain<TState, TValue>, TransitionRange<TState, TValue>>(mapping);
        }
    }
}
