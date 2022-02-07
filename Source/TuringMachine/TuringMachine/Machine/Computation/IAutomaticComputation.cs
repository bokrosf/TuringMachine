﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for starting automatic computations.
/// </summary>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <typeparam name="TConfiguration">Type of the machine's configuration.</typeparam>
public interface IAutomaticComputation<TSymbol, TConfiguration>
    where TConfiguration : notnull
{
    /// <summary>
    /// Asynchronously starts an automatically stepping computation process with the specified symbols.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    /// <returns><see cref="Task"/> that is the computation process.</returns>
    Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input);

    /// <summary>
    /// Asynchronously starts an automatically stepping computation process with the specified symbols and a constraint.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    /// <param name="constraint">A constraint that must be enforced during the computation process.</param>
    /// <returns><see cref="Task"/> that is the computation process.</returns>
    Task StartAutomaticComputationAsync(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint);

    /// <summary>
    /// Starts an automatically stepping computation process with the specified symbols.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input);

    /// <summary>
    /// Starts an automatically stepping computation process, with the specified symbols and constraint.
    /// </summary>
    /// <param name="input">Symbols that the tape is initialized with.</param>
    /// <param name="constraint">A constraint that must be enforced during the computation process.</param>
    void StartAutomaticComputation(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint);

    /// <summary>
    /// Aborts the computation that is in progress.
    /// </summary>
    void RequestAbortion();
}