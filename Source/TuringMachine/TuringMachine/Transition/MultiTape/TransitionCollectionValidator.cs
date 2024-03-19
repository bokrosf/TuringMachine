using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Check validity of multi-tape transaction collections.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class TransitionCollectionValidator<TState, TSymbol>
{
    /// <summary>
    /// Validates the given transition collection.
    /// </summary>
    /// <param name="transitions">Transition collection to be validated.</param>
    /// <exception cref="NonDeterministicTransitionException">Thrown when the collection contains a transition domain more than once.</exception>
    /// <exception cref="InvalidStateInTransitionException">Thrown when the collection contains a transition with an invalid state.</exception>
    /// <exception cref="MissingStateException">Thrown when the collection does not contain an obligatory state.</exception>
    public void Validate(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        CheckStates(transitions);
        CheckDeterminism(transitions);
        CheckTapeCount(transitions);
    }
    
    private void CheckDeterminism(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        var distinctDomains = new HashSet<TransitionDomain<TState, TSymbol>>();

        foreach (var t in transitions)
        {
            TransitionDomain<TState, TSymbol> domain = new TransitionDomain<TState, TSymbol>(t.State.Domain, t.Tapes.Select(tape => tape.Domain));
            
            if (distinctDomains.Contains(domain))
            {
                throw new NonDeterministicTransitionException($"Transition domains must be unique. Transition={t}.");
            }

            distinctDomains.Add(domain);
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

    private void CheckTapeCount(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        if (!transitions.Any())
        {
            throw new ArgumentException("At least one transition");
        }

        int tapeCount = transitions.First().Tapes.Count;

        if (transitions.Skip(1).Any(t => t.Tapes.Count != tapeCount))
        {
            throw new DifferentTransitionTapeCountException($"All transitions must have the same tape count. TapeCount={tapeCount}.");
        }
    }

    private void CheckInitialStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        if (!transitions.Any(t => t.State.Domain == State<TState>.Initial))
        {
            throw new MissingStateException($"At least one transition domain must contain {nameof(State<TState>.Initial)} state.");
        }
    }

    private void CheckAcceptStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        if (!transitions.Any(t => t.State.Range == State<TState>.Accept))
        {
            throw new MissingStateException($"At least one transition range must contain {nameof(State<TState>.Accept)} state.");
        }
    }

    private void CheckStateOfDomain(Transition<TState, TSymbol> transition)
    {
        if (GetInvalidStatesOfDomain().Contains(transition.State.Domain))
        {
            throw new InvalidStateInTransitionException(
                $"Only {nameof(State<TState>.Initial)} special state can appear in transition domain. Transition={transition}.");
        }
    }

    private void CheckStateOfRange(Transition<TState, TSymbol> transition)
    {
        if (transition.State.Range == State<TState>.Initial)
        {
            throw new InvalidStateInTransitionException(
                $"{nameof(State<TState>.Initial)} state must not appear in transition range. Transition={transition}.");
        }
    }

    private IEnumerable<State<TState>> GetInvalidStatesOfDomain()
    {
        yield return State<TState>.Accept;
        yield return State<TState>.Reject;
    }
}