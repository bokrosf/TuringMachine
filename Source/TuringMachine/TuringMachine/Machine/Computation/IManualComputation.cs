using System.Collections.Generic;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for controlling manual computations.
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface IManualComputation<TSymbol>
{
    /// <summary>
    /// Starts a manually steppable computation process with the specified symbols.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    void StartManualComputation(IEnumerable<Symbol<TSymbol>> input);

    /// <summary>
    /// Transitions the machine from it's current state to the next.
    /// </summary>
    /// <returns><see cref="bool"/> that indicates whether another step can be made after the current step performed.</returns>
    bool Step();

    /// <summary>
    /// Aborts the computation that is in progress.
    /// </summary>
    void RequestAbortion();
}
