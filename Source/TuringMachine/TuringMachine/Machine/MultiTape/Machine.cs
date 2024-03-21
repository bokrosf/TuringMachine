using System;
using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.MultiTape;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Machine.MultiTape;

/// <summary>
/// Represents a multi-tape Turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : Machine<
    TState, 
    TSymbol, 
    Transition<TState, TSymbol>,
    ComputationRequest<TState, TSymbol>>
{
    private Tape<TSymbol>[] tapes;
    private TransitionTable<TState, TSymbol>? transitionTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="Machine{TState, TSymbol}"/> class.
    /// </summary>
    public Machine()
    {
        tapes = Array.Empty<Tape<TSymbol>>();
    }

    protected override void InitializeComputation(ComputationMode computationMode, ComputationRequest<TState, TSymbol> request)
    {
        lock (computationLock)
        {
            if (computation != null)
            {
                throw new InvalidOperationException($"A(n) {computation.Mode} computation is already in progress.");
            }

            if (request.TransitionTable.TapeCount < 1)
            {
                throw new ArgumentException("Tape count must be greater than zero.", nameof(transitionTable));
            }

            tapes = Enumerable.Range(1, request.TransitionTable.TapeCount).Select(i => new Tape<TSymbol>()).ToArray();
            tapes[0] = new Tape<TSymbol>(request.Input);
            transitionTable = request.TransitionTable;
            computation = new(computationMode, Aborted: false);
        }
    }

    protected override void TransitToNextConfiguration()
    {
        TransitionDomain<TState, TSymbol> domain = new TransitionDomain<TState, TSymbol>(state, tapes.Select(t => t.CurrentSymbol));
        TransitionRange<TState, TSymbol> range = transitionTable![domain];
        state = range.State;
        ApplyTapeTransitions(range.Tapes);
        
        Transition<TState, TSymbol> transition = new(
            new StateTransition<TState>(domain.State, range.State),
            domain.TapeSymbols.Zip(range.Tapes, (domain, range) => new TapeTransition<TSymbol>(domain, range.Symbol, range.TapeHeadDirection)));

        OnStepped(new SteppedEventArgs<Transition<TState, TSymbol>>(transition));
    }

    protected override void CleanupComputation()
    {
        foreach (var t in tapes)
        {
            t.Clear();
        }

        tapes = Array.Empty<Tape<TSymbol>>();
        transitionTable = null;

        lock (computationLock)
        {
            computation = null;
        }
    }

    protected override ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs()
    {
        return new ComputationTerminatedEventArgs<TState, TSymbol>(state, tapes.First());
    }

    protected override ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(Exception? ex)
    {
        return new ComputationAbortedEventArgs<TState, TSymbol>(state, tapes.First(), ex);
    }

    private void ApplyTapeTransitions(IReadOnlyList<TapeTransitionRange<TSymbol>> tapeTransitions)
    {
        for (int i = 0; i < tapeTransitions.Count; i++)
        {
            tapes[i].CurrentSymbol = tapeTransitions[i].Symbol;
            tapes[i].MoveHeadInDirection(tapeTransitions[i].TapeHeadDirection);
        }
    }
}
