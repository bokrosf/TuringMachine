using System.Threading;
using System.Threading.Tasks;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Defines methods for starting automatic computations.
/// </summary>
/// <typeparam name="TComputationRequest">Arguments of a computation initiation.</typeparam>
public interface IAutomaticComputation<TComputationRequest> where TComputationRequest : notnull
{
    /// <summary>
    /// Asynchronously starts an automatically stepping computation process.
    /// </summary>
    /// <param name="request">Arguments of a computation initiation.</param>
    /// <returns><see cref="Task"/> that is the computation process.</returns>
    Task StartAutomaticAsync(TComputationRequest request);
    
    /// <summary>
    /// Asynchronously starts an automatically stepping computation process that can be cancelled.
    /// </summary>
    /// <param name="request">Arguments of a computation initiation.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects and threads to receive notification of cancellation.</param>
    /// <returns><see cref="Task"/> that is the computation process.</returns>
    Task StartAutomaticAsync(TComputationRequest request, CancellationToken cancellationToken);
}
