using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TuringMachine.Transition.SingleTape;

/// <summary>
/// Represents the mapping of transition domains and ranges that can be used by a machine for transitioning one configuration to the next.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public sealed class TransitionTable<TState, TSymbol>
{
    private readonly ReadOnlyDictionary<TransitionDomain<TState, TSymbol>, TransitionRange<TState, TSymbol>> transitions;

    /// <summary>
    /// Gets the range that belongs to the given transition domain.
    /// </summary>
    /// <param name="domain">Domain of a transitions.</param>
    /// <returns><see cref="TransitionRange{TState, TSymbol}"/> that belongs to the given transition domain.</returns>
    /// <exception cref="TransitionDomainNotFoundException">Thrown when the table does not contain any range belonging the given domain.</exception>
    internal TransitionRange<TState, TSymbol> this[TransitionDomain<TState, TSymbol> domain]
    {
        get
        {
            return transitions.TryGetValue(domain, out var range)
                ? range
                : throw new TransitionDomainNotFoundException($"Not found domain={domain}.");
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransitionTable{TState, TSymbol}"/> class with the given collection of transitions.
    /// </summary>
    /// <param name="transitions">Transitions.</param>
    /// <exception cref="DuplicateTransitionException">Thrown when the collection contains a duplicate transition.</exception>
    /// <exception cref="NonDeterministicTransitionException">Thrown when the collection contains a transition domain more than once.</exception>
    /// <exception cref="InvalidStateInTransitionException">Thrown when the collection contains a transition with an invalid state.</exception>
    /// <exception cref="MissingStateException">Thrown when the collection does not contain an obligatory state.</exception>
    public TransitionTable(IEnumerable<Transition<TState, TSymbol>> transitions)
    {
        new TransitionCollectionValidator<TState, TSymbol>().Validate(transitions);
        this.transitions = new(transitions.ToDictionary(t => t.Domain, t => t.Range));
    }
}
