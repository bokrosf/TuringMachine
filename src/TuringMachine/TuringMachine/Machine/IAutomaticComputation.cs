﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.ComputationConstraint;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Defines methods for starting automatic computations.
    /// </summary>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IAutomaticComputation<TState, TSymbol>
    {
        /// <summary>
        /// Asynchronously starts an automatically stepping computation process with the specified symbols.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input);

        /// <summary>
        /// Asynchronously starts an automatically stepping computation process with the specified symbols and a constraint.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="constraint">A constraint that must be enforced during the computation process.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, ComputationConstraint<TState, TSymbol> constraint);

        /// <summary>
        /// Starts an automatically stepping computation process with the specified symbols.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input);

        /// <summary>
        /// Starts an automatically stepping computation process, with the specified symbols and constraint.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="constraint">A constraint that must be enforced during the computation process.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input, ComputationConstraint<TState, TSymbol> constraint);
    }
}
