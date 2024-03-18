using System;
using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine.Computation;
using TuringMachine.Transition.MultiTape;

namespace TuringMachine.Machine.MultiTape;

/// <summary>
/// Represents a multi-tape turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : Machine<TState, TSymbol, Transition<TState, TSymbol>>
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

    protected override void InitializeComputation(ComputationMode computationMode, IEnumerable<Symbol<TSymbol>> input)
    {
        lock (computationLock)
        {
            if (computation != null)
            {
                throw new InvalidOperationException($"A(n) {computation.Mode} computation is already in progress.");
            }

            tapes[0] = new Tape<TSymbol>(input);
            computation = new(computationMode, IsAborted: false);
        }
    }

    protected override void TransitToNextState()
    {
        TransitionDomain<TState, TSymbol> domain = new TransitionDomain<TState, TSymbol>(state, tapes.Select(t => t.CurrentSymbol));
        TransitionRange<TState, TSymbol> range = transitionTable[domain];
        TransitToNextTapeSymbols(range.Tapes);
        Transition<TState, TSymbol> transition = new(
            new StateTransition<TState>(domain.State, range.State),
            GetTapeTransitions(domain.TapeSymbols, range.Tapes));

        OnStepped(new SteppedEventArgs<Transition<TState, TSymbol>>(transition));
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

    protected override ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs()
    {
        return new ComputationTerminatedEventArgs<TState, TSymbol>(state, tapes.First());
    }

    protected override ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(Exception? ex)
    {
        return new ComputationAbortedEventArgs<TState, TSymbol>(state, tapes.First(), ex);
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
