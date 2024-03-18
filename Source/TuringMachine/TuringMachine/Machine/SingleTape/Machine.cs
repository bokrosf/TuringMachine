using System;
using System.Collections.Generic;
using TuringMachine.Machine.Computation;
using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine.SingleTape;

/// <summary>
/// Represents a single-tape turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : Machine<TState, TSymbol, Transition<TState, TSymbol>>
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

    protected override void InitializeComputation(ComputationMode computationMode, IEnumerable<Symbol<TSymbol>> input)
    {
        lock (computationLock)
        {
            if (computation != null)
            {
                throw new InvalidOperationException($"A(n) {computation.Mode} computation is already in progress.");
            }

            tape = new Tape<TSymbol>(input);
            computation = new(computationMode, IsAborted: false);
        }
    }

    protected override void TransitToNextState()
    {
        TransitionDomain<TState, TSymbol> domain = (state, tape.CurrentSymbol);
        TransitionRange<TState, TSymbol> range = transitionTable[domain];
        state = range.State;
        tape.CurrentSymbol = range.Symbol;
        tape.MoveHeadInDirection(range.HeadDirection);
        Transition<TState, TSymbol> transition = (domain, range);
        OnStepped(new SteppedEventArgs<Transition<TState, TSymbol>>(transition));
    }

    protected override void CleanupComputation()
    {
        tape.Clear();

        lock (computationLock)
        {
            computation = null;
        }
    }

    protected override ComputationTerminatedEventArgs<TState, TSymbol> CreateComputationTerminatedEventArgs()
    {
        return new ComputationTerminatedEventArgs<TState, TSymbol>(state,tape);
    }

    protected override ComputationAbortedEventArgs<TState, TSymbol> CreateComputationAbortedEventArgs(Exception? ex)
    {
        return new ComputationAbortedEventArgs<TState, TSymbol>(state, tape, ex);
    }
}
