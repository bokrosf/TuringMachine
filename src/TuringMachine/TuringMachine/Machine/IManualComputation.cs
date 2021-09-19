using System.Collections.Generic;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Defines methods for controlling manual computations.
    /// </summary>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public interface IManualComputation<TSymbol>
    {
        /// <summary>
        /// Starts a manually steppable computation process with the given symbols.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input);

        /// <summary>
        /// Starts a manually steppable computation process, with the given symbols, that can not take more steps 
        /// than the given maximum number of steps.
        /// </summary>
        /// <param name="input">Symbols that the tape is initialized with.</param>
        /// <param name="maxStepCount">Maximum number of steps the process can take before terminated.</param>
        void StartComputation(IEnumerable<Symbol<TSymbol>> input, int maxStepCount);
        
        /// <summary>
        /// Transitions the machine from it's current state to the next.
        /// </summary>
        /// <returns><see cref="bool"/> that indicates whether another step can be made after the current step performed.</returns>
        bool Step();

        /// <summary>
        /// Aborts the computation that is in progress.
        /// </summary>
        void Abort();
    }
}
