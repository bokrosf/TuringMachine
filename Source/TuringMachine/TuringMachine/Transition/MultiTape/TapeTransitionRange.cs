using TuringMachine.Machine;

namespace TuringMachine.Transition.MultiTape;

/// <summary>
/// Represents the effects of a machine transition that occur for a tape. 
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <param name="Symbol">The current symbol pointed by the tape's head is overwritten by this symbol.</param>
/// <param name="TapeHeadDirection">Direction of the tape head's movement.</param>
internal record TapeTransitionRange<TSymbol>(Symbol<TSymbol> Symbol, TapeHeadDirection TapeHeadDirection);