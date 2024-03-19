using System;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Transition;

namespace TuringMachine.Machine;

public abstract class Machine<TState, TSymbol, TTransition, TComputationRequest> :
    IAutomaticComputation<TComputationRequest>,
    IManualComputation<TComputationRequest>,
    IComputationTracking<TState, TSymbol, TTransition>
        where TTransition : notnull
        where TComputationRequest : notnull
{
    public event EventHandler<SteppedEventArgs<TTransition>>? Stepped;
    public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
    public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

    protected State<TState> state;
    protected readonly object computationLock;
    protected Computation.Computation? computation;
    private readonly object manualComputationLock;

    protected Machine()
    {
        computationLock = new object();
        manualComputationLock = new object();
        state = State<TState>.Initial;
    }

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
                if (computation == null || computation.IsAborted)
                {
                    return;
                }

                computation = computation with { IsAborted = true };
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

                if (computation.IsAborted)
                {
                    return false;
                }
            }

            return PerformStep();
        }
    }

    protected abstract void InitializeComputation(ComputationMode computationMode, TComputationRequest request);

    protected abstract void TransitToNextState();
    protected abstract void CleanupComputation();
    protected abstract ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs();
    protected abstract ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(Exception? ex);

    protected void OnStepped(SteppedEventArgs<TTransition> eventArgs)
    {
        Stepped?.Invoke(this, eventArgs);
    }

    protected void OnComputationTerminated(ComputationTerminatedEventArgs<TState, TSymbol> eventArgs)
    {
        ComputationTerminated?.Invoke(this, eventArgs);
    }

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
            if (IsAborted())
            {
                AbortComputation();
                return false;
            }

            try
            {
                TransitToNextState();
            }
            catch (TransitionDomainNotFoundException)
            {
                state = State<TState>.Reject;
            }

            if (state.IsFinishState)
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

    private bool IsAborted()
    {
        lock (computationLock)
        {
            return computation?.IsAborted ?? true;
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
