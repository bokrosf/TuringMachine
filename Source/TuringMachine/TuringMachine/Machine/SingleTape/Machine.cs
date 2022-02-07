using System;
using System.Collections.Generic;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using TuringMachine.Transition;
using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine.SingleTape;

/// <summary>
/// Represents a single-tape turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : 
    Machine<
        TState, 
        TSymbol,
        Computation.SingleTape.ComputationState<TState, TSymbol>, 
        TransitionDomain<TState, TSymbol>, 
        Transition<TState, TSymbol>>
{
    private Tape<TSymbol> tape;
    private readonly TransitionTable<TState, TSymbol> transitionTable;

    /// <summary>
    /// Initializes a new instance of <see cref="SingleTapeMachine{TState, TSymbol}"/> class with the given transition table.
    /// </summary>
    /// <param name="transitionTable">Table that contains the performable transitions.</param>
    public Machine(TransitionTable<TState, TSymbol> transitionTable)
    {
        tape = new Tape<TSymbol>();
        this.transitionTable = transitionTable;
    }

    protected override void InitializeComputation(
        ComputationMode computationMode, 
        IEnumerable<Symbol<TSymbol>> input, 
        IComputationConstraint<IReadOnlyComputationState<TransitionDomain<TState, TSymbol>>>? constraint)
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

    protected override void TransitToNextState()
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

    protected override void Terminate()
    {
        computation!.State.StopDurationWatch();
        ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(
            computation!.State.AsReadOnly(),
            computation.State.Configuration.State,
            tape);

        CleanupComputation();
        OnComputationTerminated(eventArgs);
    }

    protected override void CleanupComputation()
    {
        tape.Clear();

        lock (computationLock)
        {
            computation = null;
        }
    }

    protected override bool CanTerminate()
    {
        return computation!.State.Configuration.State.IsFinishState;
    }

    protected override void AbortComputation(Exception? ex, ConstraintViolation? violation)
    {
        computation!.State.StopDurationWatch();
        ComputationAbortedEventArgs<TState, TSymbol> eventArgs = new(
            computation!.State.AsReadOnly(),
            computation.State.Configuration.State, 
            tape, 
            ex,
            violation);

        CleanupComputation();
        OnComputationAborted(eventArgs);
    }
}
