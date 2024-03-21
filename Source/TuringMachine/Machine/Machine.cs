using System;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Transition;

namespace TuringMachine.Machine;

/// <summary>
/// Represents a Turing machine that can execute computations.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
/// <typeparam name="TTransition">Type of the configuration transition.</typeparam>
/// <typeparam name="TComputationRequest">Arguments of a computation initiation.</typeparam>
public abstract class Machine<TState, TSymbol, TTransition, TComputationRequest> :
    IAutomaticComputation<TComputationRequest>,
    IManualComputation<TComputationRequest>,
    IComputationTracking<TState, TSymbol, TTransition>
        where TTransition : notnull
        where TComputationRequest : notnull
{    
    protected State<TState> state;
    protected readonly object computationLock;
    protected Computation.Computation? computation;
    private readonly object manualComputationLock;

    /// <summary>
    /// Initializes a new instance of the <see cref="Machine{TState, TSymbol, TTransition, TComputationRequest}"/> class.
    /// </summary>
    protected Machine()
    {
        computationLock = new object();
        manualComputationLock = new object();
        state = State<TState>.Initial;
    }

	public event EventHandler<SteppedEventArgs<TTransition>>? Stepped;
	public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
	public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

	public Task StartAutomaticAsync(TComputationRequest request)
    {
        return StartAutomaticAsync(request, CancellationToken.None);
    }

    public Task StartAutomaticAsync(TComputationRequest request, CancellationToken cancellationToken)
    {
        InitializeComputation(ComputationMode.Automatic, request);

        return Task.Run(() => Compute(cancellationToken));
    }

    public void RequestAbortion()
    {
        lock (manualComputationLock)
        {
            ComputationMode mode = default;

            lock (computationLock)
            {
                if (computation == null || computation.Aborted)
                {
                    return;
                }

                computation = computation with { Aborted = true };
                mode = computation.Mode;
            }

            if (mode == ComputationMode.Manual)
            {
                AbortComputation();
            }
        }
    }

    public void StartManual(TComputationRequest request)
    {
        InitializeComputation(ComputationMode.Manual, request);
    }

    public bool Step()
    {
        lock (manualComputationLock)
        {
            lock (computationLock)
            {
                if (computation?.Mode != ComputationMode.Manual)
                {
                    throw new InvalidOperationException($"{(computation?.Mode)?.ToString() ?? "<null>"} computation mode can not be stepped manually.");
                }

                if (computation.Aborted)
                {
                    return false;
                }
            }

            return PerformStep();
        }
    }

    /// <summary>
    /// Initializes the computation by setting up the state, tapes and other components necessary for the computation.
    /// </summary>
    /// <param name="computationMode">Computation execution mode.</param>
    /// <param name="request">Arguments of a computation initiation.</param>
    protected abstract void InitializeComputation(ComputationMode computationMode, TComputationRequest request);

    /// <summary>
    /// Transits to the next configuration based on the current configuration. Triggers the <see cref="Stepped"/> event.
    /// </summary>
    protected abstract void TransitToNextConfiguration();
    
    /// <summary>
    /// Cleans up computation details, tapes. Resets the machine to the state where a new computation can be started.
    /// </summary>
    protected abstract void CleanupComputation();
    
    /// <summary>
    /// Creates the arguments needed for termination event triggering.
    /// </summary>
    protected abstract ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs();
    
    /// <summary>
    /// Creates the arguments needed for abortion event triggering.
    /// </summary>
    /// <param name="ex"></param>
    protected abstract ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(Exception? ex);

    /// <summary>
    /// Triggers the <see cref="Stepped"/> event.
    /// </summary>
    /// <param name="eventArgs">Event arguments.</param>
    protected void OnStepped(SteppedEventArgs<TTransition> eventArgs)
    {
        Stepped?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Triggers the <see cref="ComputationTerminated"/> event.
    /// </summary>
    /// <param name="eventArgs">Event arguments</param>
    protected void OnComputationTerminated(ComputationTerminatedEventArgs<TState, TSymbol> eventArgs)
    {
        ComputationTerminated?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Triggers the <see cref="ComputationAborted"/> event.
    /// </summary>
    /// <param name="eventArgs">Event arguments.</param>
    protected void OnComputationAborted(ComputationAbortedEventArgs<TState, TSymbol> eventArgs)
    {
        ComputationAborted?.Invoke(this, eventArgs);
    }

    private void Terminate()
    {
        ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = CreateComputationTerminatedEventArgs();
        CleanupComputation();
        OnComputationTerminated(eventArgs);
    }

    private bool PerformStep()
    {
        try
        {
            if (Aborted())
            {
                AbortComputation();
                return false;
            }

            try
            {
                TransitToNextConfiguration();
            }
            catch (TransitionDomainNotFoundException)
            {
                state = State<TState>.Reject;
            }

            if (state.Finish)
            {
                Terminate();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            AbortComputation(ex);
            return false;
        }
    }

    private bool Aborted()
    {
        lock (computationLock)
        {
            return computation?.Aborted ?? true;
        }
    }

    private void Compute(CancellationToken cancellationToken)
    {
        do 
        {
            if (cancellationToken.IsCancellationRequested)
            {
                AbortComputation();
            }
        } while (PerformStep());
    }

    private void AbortComputation() => AbortComputation(ex: null);

    private void AbortComputation(Exception? ex)
    {
        ComputationAbortedEventArgs<TState, TSymbol> eventArgs = CreateComputationAbortedEventArgs(ex);
        CleanupComputation();
        OnComputationAborted(eventArgs);
    }
}
