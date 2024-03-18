using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for starting automatic computations.
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public interface IAutomaticComputation<TSymbol>
{
    /// <summary>
    /// Asynchronously starts an automatically stepping computation process with the specified symbols.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    /// <returns><see cref="Task"/> that is the computation process.</returns>
    Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input);

    /// <summary>
    /// Aborts the computation that is in progress.
    /// </summary>
    void RequestAbortion();
}
