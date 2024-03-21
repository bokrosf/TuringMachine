using System.Collections.Generic;
using System.Linq;

namespace TuringMachine.Transition.SingleTape;

using ErrorCollection = ICollection<string>;

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
    /// <returns><see cref="ValidationResult"/> that contains whether it was successful and errors if validation failed.</returns>
    public ValidationResult Validate(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        ValidationResult result = new ValidationResult();
        CheckStates(transitions, result.Errors);
        CheckDeterminism(transitions, result.Errors);

        return result;
    }

    private void CheckDeterminism(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        var distinctDomains = new HashSet<TransitionDomain<TState, TSymbol>>();

        foreach (var t in transitions)
        {
            if (distinctDomains.Contains(t.Domain))
            {
                errors.Add($"Transition domains must be unique. Transition={t}.");
            }

            distinctDomains.Add(t.Domain);
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

    private void CheckInitialStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        if (!transitions.Any(t => t.Domain.State == State<TState>.Initial))
        {
            errors.Add($"At least one transition domain must contain {nameof(State<TState>.Initial)} state.");
        }
    }

    private void CheckAcceptStatePresence(IEnumerable<Transition<TState, TSymbol>> transitions, ErrorCollection errors)
    {
        if (!transitions.Any(t => t.Range.State == State<TState>.Accept))
        {
            errors.Add($"At least one transition range must contain {nameof(State<TState>.Accept)} state.");
        }
    }

    private void CheckStateOfDomain(Transition<TState, TSymbol> transition, ErrorCollection errors)
    {
        if (GetInvalidStatesOfDomain().Contains(transition.Domain.State))
        {
            errors.Add($"Only {nameof(State<TState>.Initial)} special state can appear in transition domain. Transition={transition}.");
        }
    }

    private void CheckStateOfRange(Transition<TState, TSymbol> transition, ErrorCollection errors)
    {
        if (transition.Range.State == State<TState>.Initial)
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
