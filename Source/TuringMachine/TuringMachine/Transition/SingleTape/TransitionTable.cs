using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TuringMachine.Transition.SingleTape;

/// <summary>
/// Represents the mapping of transition domains and ranges that can be used by a single-tape machine for transitioning
/// from one configuration to the next.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public sealed class TransitionTable<TState, TSymbol>
{
    private readonly ReadOnlyDictionary<TransitionDomain<TState, TSymbol>, TransitionRange<TState, TSymbol>> transitions;

	/// <summary>
	/// Initializes a new instance of the <see cref="TransitionTable{TState, TSymbol}"/> class with the given collection of transitions.
	/// </summary>
	/// <param name="transitions">Transitions.</param>
	/// <exception cref="InvalidTransitionCollectionException">The transition collection is invalid.</exception>
	public TransitionTable(IEnumerable<Transition<TState, TSymbol>> transitions)
	{
		ValidationResult validationResult = new TransitionCollectionValidator<TState, TSymbol>().Validate(transitions);

		if (!validationResult.Valid)
		{
			throw new InvalidTransitionCollectionException("The transition collection is invalid.");
		}

		this.transitions = new(transitions.ToDictionary(t => t.Domain, t => t.Range));
	}

	/// <summary>
	/// Gets the range that belongs to the given transition domain.
	/// </summary>
	/// <param name="domain">Domain of a transition.</param>
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
}
