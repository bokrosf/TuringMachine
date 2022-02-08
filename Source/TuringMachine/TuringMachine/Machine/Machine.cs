using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;

namespace TuringMachine.Machine;

public abstract class Machine<TState, TSymbol, TComputationState, TConfiguration, TTransition> :
    IAutomaticComputation<TSymbol, TConfiguration>,
    IManualComputation<TSymbol, TConfiguration>,
    IComputationTracking<TState, TSymbol, TTransition>
        where TComputationState : ComputationState<TConfiguration>
        where TConfiguration : notnull
        where TTransition : notnull
{
    public event EventHandler<SteppedEventArgs<TTransition>>? Stepped;
    public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
    public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

    protected readonly object computationLock;
    protected Computation<TComputationState, TConfiguration>? computation;
    private readonly object manualComputationLock;

    protected Machine()
    {
        computationLock = new object();
        manualComputationLock = new object();
    }

    public Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input)
    {
        return Task.Run(() => StartAutomaticComputation(input));
    }

    public Task StartAutomaticComputationAsync(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint)
    {
        return Task.Run(() => StartAutomaticComputation(input, constraint));
    }

    public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input)
    {
        InitializeComputation(ComputationMode.Automatic, input, constraint: null);
        Compute();
    }

    public void StartAutomaticComputation(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint)
    {
        InitializeComputation(ComputationMode.Automatic, input, constraint);
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
        InitializeComputation(ComputationMode.Manual, input, constraint: null);
    }

    public void StartManualComputation(
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>> constraint)
    {
        InitializeComputation(ComputationMode.Manual, input, constraint);
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
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<IReadOnlyComputationState<TConfiguration>>? constraint);

    protected abstract void TransitToNextState();
    protected abstract void CleanupComputation();
    protected abstract bool CanTerminate();
    protected abstract ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs();
    protected abstract ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(
        Exception? ex, 
        ConstraintViolation? violation);

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
        computation!.State.StopDurationWatch();
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

            TransitToNextState();

            if (CanTerminate())
            {
                Terminate();
                return false;
            }

            if (computation!.Constraint?.Enforce(computation.State.AsReadOnly()) is ConstraintViolation violation)
            {
                AbortComputation(violation);
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

    private void AbortComputation() => AbortComputation(ex: null, violation: null);
    private void AbortComputation(Exception? ex) => AbortComputation(ex, violation: null);
    private void AbortComputation(ConstraintViolation? violation) => AbortComputation(ex: null, violation);

    private void AbortComputation(Exception? ex, ConstraintViolation? violation)
    {
        computation!.State.StopDurationWatch();
        ComputationAbortedEventArgs<TState, TSymbol> eventArgs = CreateComputationAbortedEventArgs(ex, violation);
        CleanupComputation();
        OnComputationAborted(eventArgs);
    }
}
