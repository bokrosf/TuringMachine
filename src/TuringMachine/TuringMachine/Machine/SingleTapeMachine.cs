﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using TuringMachine.Transition;
using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine;

/// <summary>
/// Represents a single-tape turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class SingleTapeMachine<TState, TSymbol> :
    IAutomaticComputation<TState, TSymbol>,
    IManualComputation<TState, TSymbol>,
    IComputationTracking<TState, TSymbol>
{
    public event EventHandler<SteppedEventArgs<TState, TSymbol>>? Stepped;
    public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
    public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

    private readonly object computationLock;
    private readonly object manualComputationLock;
    private Computation<TState, TSymbol>? computation;
    private Tape<TSymbol> tape;
    private readonly TransitionTable<TState, TSymbol> transitionTable;

    /// <summary>
    /// Initializes a new instance of <see cref="SingleTapeMachine{TState, TSymbol}"/> class with the given transition table.
    /// </summary>
    /// <param name="transitionTable">Table that contains the performable transitions.</param>
    public SingleTapeMachine(TransitionTable<TState, TSymbol> transitionTable)
    {
        computationLock = new object();
        manualComputationLock = new object();
        tape = new Tape<TSymbol>();
        this.transitionTable = transitionTable;
    }

    public Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input)
    {
        return Task.Run(() => StartAutomaticComputation(input));
    }

    public Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input, IComputationConstraint<TState, TSymbol> constraint)
    {
        return Task.Run(() => StartAutomaticComputation(input, constraint));
    }

    public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input)
    {
        InitializeComputation(ComputationMode.Automatic, input, constraint: null);
        Compute();
    }

    public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input, IComputationConstraint<TState, TSymbol> constraint)
    {
        InitializeComputation(ComputationMode.Automatic, input, constraint);
        Compute();
    }

    public void StartManualComputation(IEnumerable<Symbol<TSymbol>> input)
    {
        InitializeComputation(ComputationMode.Manual, input, constraint: null);
    }

    public void StartManualComputation(IEnumerable<Symbol<TSymbol>> input, IComputationConstraint<TState, TSymbol> constraint)
    {
        InitializeComputation(ComputationMode.Manual, input, constraint);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Computation has not started manually.</exception>
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

    private void InitializeComputation(
        ComputationMode computationMode,
        IEnumerable<Symbol<TSymbol>> input,
        IComputationConstraint<TState, TSymbol>? constraint)
    {
        lock (computationLock)
        {
            if (computation != null)
            {
                throw new InvalidOperationException($"A(n) {computation.Mode} computation is already in progress.");
            }

            tape = new Tape<TSymbol>(input);
            computation = new(computationMode, new(tape.CurrentSymbol), constraint, IsAborted: false);
        }

        computation.State.StartDurationWatch();
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

    private void TransitToNextState()
    {
        TransitionDomain<TState, TSymbol> domainBeforeTransition = computation!.State.Configuration;

        try
        {
            TransitionRange<TState, TSymbol> range = transitionTable[domainBeforeTransition];
            tape.CurrentSymbol = range.Symbol;
            tape.MoveHeadInDirection(range.HeadDirection);
            computation.State.UpdateConfiguration((range.State, tape.CurrentSymbol));
            Transition<TState, TSymbol> transition = (domainBeforeTransition, range);
            OnStepped(new(computation.State.AsReadOnly(), transition));
        }
        catch (TransitionDomainNotFoundException)
        {
            computation.State.UpdateConfiguration((State<TState>.Reject, domainBeforeTransition.Symbol));
        }
    }

    private bool CanTerminate()
    {
        return computation!.State.Configuration.State.IsFinishState;
    }

    private void AbortComputation() => AbortComputation(exception: null, constraintViolation: null);
    private void AbortComputation(Exception? ex) => AbortComputation(exception: ex, constraintViolation: null);
    private void AbortComputation(ConstraintViolation? violation) => AbortComputation(exception: null, constraintViolation: violation);

    private void AbortComputation(Exception? exception, ConstraintViolation? constraintViolation)
    {
        computation!.State.StopDurationWatch();
        ComputationAbortedEventArgs<TState, TSymbol> eventArgs = new(computation!.State.AsReadOnly(), tape, exception, constraintViolation);
        CleanupComputation();
        OnComputationAborted(eventArgs);
    }

    private void Terminate()
    {
        computation!.State.StopDurationWatch();
        ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(computation!.State.AsReadOnly(), tape);
        CleanupComputation();
        OnComputationTerminated(eventArgs);
    }

    private void CleanupComputation()
    {
        tape.Clear();

        lock (computationLock)
        {
            computation = null;
        }
    }

    private void OnStepped(SteppedEventArgs<TState, TSymbol> eventArgs)
    {
        Stepped?.Invoke(this, eventArgs);
    }

    private void OnComputationTerminated(ComputationTerminatedEventArgs<TState, TSymbol> eventArgs)
    {
        ComputationTerminated?.Invoke(this, eventArgs);
    }

    private void OnComputationAborted(ComputationAbortedEventArgs<TState, TSymbol> eventArgs)
    {
        ComputationAborted?.Invoke(this, eventArgs);
    }
}
