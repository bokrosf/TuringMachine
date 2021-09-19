using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Defines methods for starting automatic computations.
    /// </summary>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IAutomaticComputation<TSymbol>
    {
        /// <summary>
        /// Asynchronously starts an automatically stepping computation process with the given symbols.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input);

        /// <summary>
        /// Asynchronously starts an automatically stepping computation process with the given symbols and a token that can be used for 
        /// requesting cancellation of the process.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="cancellationToken">A token that is used for requesting a cancellation.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously starts an automatically stepping computation process, with the given symbols, that can not take more steps 
        /// than the given maximum number of steps.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="maxStepCount">Maximum number of steps the process can take before terminated.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, int maxStepCount);

        /// <summary>
        /// Asynchronously starts an automatically stepping computation process, with the given symbols, that can not take more time 
        /// than the given timeout.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="timeout">Maximum time duration the process can take before terminated.</param>
        /// <returns><see cref="Task"/> that is the computation process.</returns>
        Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, TimeSpan timeout);

        /// <summary>
        /// Starts an automatically stepping computation process with the given symbols.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input);

        /// <summary>
        /// Starts an automatically stepping computation process, with the given symbols, that can not take more steps 
        /// than the given maximum number of steps.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="maxStepCount">Maximum number of steps the process can take before terminated.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input, int maxStepCount);

        /// <summary>
        /// Starts an automatically stepping computation process, with the given symbols, that can not take more time than the given timeout.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="timeout">Maximum time duration the process can take before terminated.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input, TimeSpan timeout);
    }
}
