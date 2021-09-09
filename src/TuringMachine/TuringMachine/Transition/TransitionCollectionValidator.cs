using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition
{
    /// <summary>
    /// Check validity of transaction collections.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    internal class TransitionCollectionValidator<TState, TSymbol>
    {
        /// <summary>
        /// Validates the given transition collection.
        /// </summary>
        /// <param name="transitions">Transition collection to be validated.</param>
        /// <exception cref="NoTransitionProvidedException">Thrown when no machine transition has been provided.</exception>
        /// <exception cref="DuplicateTransitionException">Thrown when the collection contains a duplicate transition.</exception>
        /// <exception cref="NonDeterministicTransitionException">Thrown when the collection contains a transition domain more than once.</exception>
        /// <exception cref="InvalidStateInTransitionException">Thrown when the collection contains a transition with an invalid state.</exception>
        /// <exception cref="InitialStateMissingException">Throw when the collection does not contain Initial state.</exception>
        /// <exception cref="AcceptStateMissingException">Throw when the collection does not contain Accept state.</exception>
        public void Validate(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            if (!transitions.Any())
            {
                throw new NoTransitionProvidedException($"At least one transition must be provided.");
            }

            CheckDuplications(transitions);
            CheckDeterminism(transitions);
            CheckStates(transitions);
        }

        private void CheckDuplications(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            var distinctTransitions = new HashSet<Transition<TState, TSymbol>>();

            foreach (var t in transitions)
            {
                if (distinctTransitions.Contains(t))
                {
                    throw new DuplicateTransitionException($"Transitions must be unique. Transition={t}.");
                }

                distinctTransitions.Add(t);
            }
        }

        private void CheckDeterminism(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            var distinctDomains = new HashSet<TransitionDomain<TState, TSymbol>>();

            foreach (var t in transitions)
            {
                if (distinctDomains.Contains(t.Domain))
                {
                    throw new NonDeterministicTransitionException($"Transition domains must be unique. Transition={t}.");
                }

                distinctDomains.Add(t.Domain);
            }
        }

        private void CheckStates(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            CheckInitialStatePresence(transitions);
            CheckAcceptStatePresence(transitions);
            
            foreach (var t in transitions)
            {
                CheckStateOfDomain(t);
                CheckStateOfRange(t);
            }
        }

        private void CheckInitialStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            if (!transitions.Any(t => t.Domain.State == State<TState>.Initial))
            {
                throw new InitialStateMissingException($"At least one transition domain must contain {nameof(State<TState>.Initial)} state.");
            }
        }

        private void CheckAcceptStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions)
        {
            if (!transitions.Any(t => t.Range.State == State<TState>.Accept))
            {
                throw new AcceptStateMissingException($"At least one transition range must contain {nameof(State<TState>.Accept)} state.");
            }
        }

        private void CheckStateOfDomain(Transition<TState, TSymbol> transition)
        {
            if (GetInvalidStatesOfDomain().Contains(transition.Domain.State))
            {
                throw new InvalidStateInTransitionException(
                    $"Only {nameof(State<TState>.Initial)} special state can appear in transition domain. Transition={transition}.");
            }
        }

        private void CheckStateOfRange(Transition<TState, TSymbol> transition)
        {
            if (transition.Range.State == State<TState>.Initial)
            {
                throw new InvalidStateInTransitionException(
                    $"{nameof(State<TState>.Initial)} state must not appear in transition range. Transition={transition}.");
            }
        }

        private IEnumerable<State<TState>> GetInvalidStatesOfDomain()
        {
            yield return State<TState>.Accept;
            yield return State<TState>.Failure;
        }
    }
}
