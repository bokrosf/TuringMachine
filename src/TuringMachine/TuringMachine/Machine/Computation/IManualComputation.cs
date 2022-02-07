using System.Collections.Generic;
using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for controlling manual computations.
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <typeparam name="TConfiguration">Type of the machine's configuration.</typeparam>
public interface IManualComputation<TSymbol, TConfiguration>
    where TConfiguration : notnull
{
    /// <summary>
    /// Starts a manually steppable computation process with the specified symbols.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    void StartManualComputation(IEnumerable<Symbol<TSymbol>> input);

    /// <summary>
    /// Starts a manually steppable computation process, with the specified symbols and a constraint.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    /// <param name="constraint">A constraint that must be enforced during the computation process.</param>
    void StartManualComputation(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint);

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
