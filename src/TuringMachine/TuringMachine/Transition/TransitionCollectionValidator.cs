using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Check validity of transaction collections.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    internal class TransitionCollectionValidator<TState, TValue>
    {
        /// <summary>
        /// Validates the given transition collection.
        /// </summary>
        /// <param name="transitions">Transition collection to be validated.</param>
        /// <exception cref="NoTransitionProvidedException">Thrown when no machine transition has been provided.</exception>
        /// <exception cref="DuplicateTransitionException">Thrown when the collection contains a duplicate transition.</exception>
        /// <exception cref="NonDeterministicTransitionException">Thrown when the collection contains a transition domain more than once.</exception>
        /// <exception cref="InvalidStateInTransitionException">Thrown when the collection contains a transition with an invalid state.</exception>
        public void Validate(IEnumerable<Transition<TState, TValue>> transitions)
        {
            if (!transitions.Any())
            {
                throw new NoTransitionProvidedException($"At least one transition must be provided.");
            }

            CheckDuplications(transitions);
            CheckDeterminism(transitions);
            CheckStates(transitions);
        }

        private void CheckDuplications(IEnumerable<Transition<TState, TValue>> transitions)
        {
            var items = new HashSet<Transition<TState, TValue>>();

            foreach (var t in transitions)
            {
                if (items.Contains(t))
                {
                    throw new DuplicateTransitionException($"No duplicate transitions allowed in the table. Transition={t}.");
                }

                items.Add(t);
            }
        }

        private void CheckDeterminism(IEnumerable<Transition<TState, TValue>> transitions)
        {
            var domains = new HashSet<TransitionDomain<TState, TValue>>();

            foreach (var t in transitions)
            {
                if (domains.Contains(t.Domain))
                {
                    throw new NonDeterministicTransitionException($"Transition domains must be unique. Transition={t}.");
                }

                domains.Add(t.Domain);
            }
        }

        private void CheckStates(IEnumerable<Transition<TState, TValue>> transitions)
        {
            if (!transitions.Any(t => t.Domain.State == State<TState>.Initial))
            {
                throw new InitialStateMissingException($"At least one transition's domain must contain {nameof(State<TState>.Initial)} state.");
            }

            if (!transitions.Any(t => t.Range.State == State<TState>.Accept))
            {
                throw new AcceptStateMissingException($"At least one transition's range must contain {nameof(State<TState>.Accept)} state.");
            }
            
            foreach (var t in transitions)
            {
                CheckStateOfDomain(t);
                CheckStateOfRange(t);
            }
        }

        private void CheckStateOfDomain(Transition<TState, TValue> transition)
        {
            if (GetInvalidStatesOfDomain().Contains(transition.Domain.State))
            {
                throw new InvalidStateInTransitionException(
                    $"Only {nameof(State<TState>.Initial)} special state can appear in a transitions' domain. Transition={transition}.");
            }
        }

        private void CheckStateOfRange(Transition<TState, TValue> transition)
        {
            if (transition.Range.State == State<TState>.Initial)
            {
                throw new InvalidStateInTransitionException(
                    $"{nameof(State<TState>.Initial)} state must not appear in a transitions's {nameof(transition.Range)}. Transition={transition}.");
            }
        }

        private IEnumerable<State<TState>> GetInvalidStatesOfDomain()
        {
            yield return State<TState>.Accept;
            yield return State<TState>.Failure;
        }
    }
}
