using System;
using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using TuringMachine.Transition;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Machine.MultiTape;

/// <summary>
/// Represents a multi-tape turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : 
    Machine<
        TState,
        TSymbol,
        Computation.MultiTape.ComputationState<TState, TSymbol>,
        TransitionDomain<TState, TSymbol>,
        Transition<TState, TSymbol>>
{
    private readonly Tape<TSymbol>[] tapes;
    private readonly ITransitionTable<TState, TSymbol> transitionTable;

    /// <summary>
    /// Initializes a new instance of <see cref="Machine{TState, TSymbol}"/> class with the specified transition table.
    /// </summary>
    /// <param name="transitionTable">Table that contains the performable transitions.</param>
    /// <exception cref="ArgumentException">Tape count is less than one.</exception>
    public Machine(ITransitionTable<TState, TSymbol> transitionTable)
    {
        if (transitionTable.TapeCount < 1)
        {
            throw new ArgumentException("Tape count must be greater than zero.", nameof(transitionTable));
        }

        tapes = Enumerable.Range(1, transitionTable.TapeCount).Select(i => new Tape<TSymbol>()).ToArray();
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

            tapes[0] = new Tape<TSymbol>(input);
            computation = new(computationMode, new(tapes.Select(t => t.CurrentSymbol)), constraint, IsAborted: false);
        }

        computation.State.StartDurationWatch();
    }

    protected override void TransitToNextState()
    {
        TransitionDomain<TState, TSymbol> domainBeforeTransition = computation!.State.Configuration;

        try
        {
            TransitionRange<TState, TSymbol> range = transitionTable[domainBeforeTransition];
            TransitToNextTapeSymbols(range.Tapes);
            computation.State.UpdateConfiguration(new(range.State, tapes.Select(t => t.CurrentSymbol)));
            Transition<TState, TSymbol> transition = new(
                new StateTransition<TState>(domainBeforeTransition.State, range.State),
                GetTapeTransitions(domainBeforeTransition.TapeSymbols, range.Tapes));

            OnStepped(new(computation.State.AsReadOnly(), transition));
        }
        catch (TransitionDomainNotFoundException)
        {
            computation.State.UpdateConfiguration(new(State<TState>.Reject, domainBeforeTransition.TapeSymbols));
        }
    }

    protected override void Terminate()
    {
        computation!.State.StopDurationWatch();
        ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(
            computation!.State.AsReadOnly(),
            computation.State.Configuration.State,
            tapes.First());

        CleanupComputation();
        OnComputationTerminated(eventArgs);
    }

    protected override void CleanupComputation()
    {
        foreach (var t in tapes)
        {
            t.Clear();
        }

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
            tapes.First(),
            ex,
            violation);

        CleanupComputation();
        OnComputationAborted(eventArgs);
    }

    private void TransitToNextTapeSymbols(IReadOnlyList<TapeTransitionRange<TSymbol>> tapeTransitions)
    {
        for (int i = 0; i < tapeTransitions.Count; i++)
        {
            tapes[i].CurrentSymbol = tapeTransitions[i].Symbol;
            tapes[i].MoveHeadInDirection(tapeTransitions[i].TapeHeadDirection);
        }
    }

    private IEnumerable<TapeTransition<TSymbol>> GetTapeTransitions(
        IReadOnlyList<Symbol<TSymbol>> domains, 
        IReadOnlyList<TapeTransitionRange<TSymbol>> ranges)
    {
        for (int i = 0; i < domains.Count; i++)
        {
            yield return (domains[i], ranges[i].Symbol, ranges[i].TapeHeadDirection);
        }
    }
}
