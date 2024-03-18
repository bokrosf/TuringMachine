using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Transition;

namespace TuringMachine.Machine;

public abstract class Machine<TState, TSymbol, TTransition> :
    IAutomaticComputation<TSymbol>,
    IManualComputation<TSymbol>,
    IComputationTracking<TState, TSymbol, TTransition>
        where TTransition : notnull
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

    public Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input)
    {
        return Task.Run(() => StartAutomaticComputation(input));
    }

    public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input)
    {
        InitializeComputation(ComputationMode.Automatic, input);
        Compute();
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

    public void StartManualComputation(IEnumerable<Symbol<TSymbol>> input)
    {
        InitializeComputation(ComputationMode.Manual, input);
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

    protected abstract void InitializeComputation(
        ComputationMode computationMode, 
        IEnumerable<Symbol<TSymbol>> input);

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

    private void Compute()
    {
        while (PerformStep())
        {
            ;
        }
    }

    private void AbortComputation() => AbortComputation(ex: null);

    private void AbortComputation(Exception? ex)
    {
        ComputationAbortedEventArgs<TState, TSymbol> eventArgs = CreateComputationAbortedEventArgs(ex);
        CleanupComputation();
        OnComputationAborted(eventArgs);
    }
}
