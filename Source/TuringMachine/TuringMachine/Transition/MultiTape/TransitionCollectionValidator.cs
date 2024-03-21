using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.MultiTape;

using ErrorCollection = ICollection<string>;

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
    /// <returns><see cref="ValidationResult"/> that contains whether it was successful and errors if validation failed.</returns>            
    public ValidationResult Validate(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        ValidationResult result = new ValidationResult();
        CheckStates(transitions, result.Errors);
        CheckDeterminism(transitions, result.Errors);
        CheckTapeCount(transitions, result.Errors);

        return result;
    }
    
    private void CheckDeterminism(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        var distinctDomains = new HashSet<TransitionDomain<TState, TSymbol>>();

        foreach (var t in transitions)
        {
            TransitionDomain<TState, TSymbol> domain = new TransitionDomain<TState, TSymbol>(t.State.Domain, t.Tapes.Select(tape => tape.Domain));
            
            if (distinctDomains.Contains(domain))
            {
                errors.Add($"Transition domains must be unique. Transition={t}.");
            }

            distinctDomains.Add(domain);
        }
    }

    private void CheckStates(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        CheckInitialStatePresence(transitions, errors);
        CheckAcceptStatePresence(transitions, errors);

        foreach (var t in transitions)
        {
            CheckStateOfDomain(t, errors);
            CheckStateOfRange(t, errors);
        }
    }

    private void CheckTapeCount(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        if (!transitions.Any())
        {
            errors.Add("The collection must contain at least one transition.");
            return;
        }
        
        int tapeCount = transitions.First().Tapes.Count;

        if (transitions.Skip(1).Any(t => t.Tapes.Count != tapeCount))
        {
            errors.Add($"All transitions must have the same tape count. TapeCount={tapeCount}.");
        }
    }

    private void CheckInitialStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        if (!transitions.Any(t => t.State.Domain == State<TState>.Initial))
        {
            errors.Add($"At least one transition domain must contain {nameof(State<TState>.Initial)} state.");
        }
    }

    private void CheckAcceptStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        if (!transitions.Any(t => t.State.Range == State<TState>.Accept))
        {
            errors.Add($"At least one transition range must contain {nameof(State<TState>.Accept)} state.");
        }
    }

    private void CheckStateOfDomain(Transition<TState, TSymbol> transition, ErrorCollection errors)
    {
        if (GetInvalidStatesOfDomain().Contains(transition.State.Domain))
        {
            errors.Add($"Only {nameof(State<TState>.Initial)} special state can appear in transition domain. Transition={transition}.");
        }
    }

    private void CheckStateOfRange(Transition<TState, TSymbol> transition, ErrorCollection errors)
    {
        if (transition.State.Range == State<TState>.Initial)
        {
            errors.Add($"{nameof(State<TState>.Initial)} state must not appear in transition range. Transition={transition}.");
        }
    }

    private IEnumerable<State<TState>> GetInvalidStatesOfDomain()
    {
        yield return State<TState>.Accept;
        yield return State<TState>.Reject;
    }
}