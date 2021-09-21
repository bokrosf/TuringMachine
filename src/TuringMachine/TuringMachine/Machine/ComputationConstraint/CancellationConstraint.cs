using System;
using System.Threading;

namespace TuringMachine.Machine.ComputationConstraint
{
    /// <summary>
    /// Enforces that a computation can be cancelled.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class CancellationConstraint<TState, TSymbol> : ComputationConstraint<TState, TSymbol>
    {
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// Initializes a new instance of <see cref="CancellationConstraint{TState, TValue}"/> class with the specified cancellation token.
        /// </summary>
        /// <param name="cancellationToken">Token used for signaling a cancellation request.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cancellationToken"/> can not be canceled.</exception>
        public CancellationConstraint(CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                throw new ArgumentException(nameof(cancellationToken), "The specified token must be cancellable.");
            }
            
            this.cancellationToken = cancellationToken;
        }

        /// <inheritdoc/>
        /// <exception cref="ComputationCancellationRequestedException">
        /// Thrown when cancellation requested and computation has not finished.
        /// </exception>
        public override void Enforce(ComputationState<TState, TSymbol> computationState)
        {
            if (cancellationToken.IsCancellationRequested && !IsComputationFinished(computationState))
            {
                throw new ComputationCancellationRequestedException();
            }
        }
    }
}
