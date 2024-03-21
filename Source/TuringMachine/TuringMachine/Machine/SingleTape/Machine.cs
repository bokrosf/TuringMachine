using System;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.SingleTape;
using TuringMachine.Transition.SingleTape;

namespace TuringMachine.Machine.SingleTape;

/// <summary>
/// Represents a single-tape Turing machine.
/// </summary>
/// <typeparam name="TState">Type of the machine's state.</typeparam>
/// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
public class Machine<TState, TSymbol> : Machine<
    TState,
    TSymbol,
    Transition<TState, TSymbol>,
    ComputationRequest<TState, TSymbol>>
{
    private Tape<TSymbol> tape;
    private TransitionTable<TState, TSymbol>? transitionTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleTapeMachine{TState, TSymbol}"/> class.
    /// </summary>
    public Machine()
    {
        tape = new Tape<TSymbol>();
    }

    protected override void InitializeComputation(ComputationMode computationMode, ComputationRequest<TState, TSymbol> request)
    {
        lock (computationLock)
        {
            if (computation != null)
            {
                throw new InvalidOperationException($"A(n) {computation.Mode} computation is already in progress.");
            }

            tape = new Tape<TSymbol>(request.Input);
            transitionTable = request.TransitionTable;
            computation = new(computationMode, IsAborted: false);
        }
    }

    protected override void TransitToNextConfiguration()
    {
        TransitionDomain<TState, TSymbol> domain = (state, tape.CurrentSymbol);
        TransitionRange<TState, TSymbol> range = transitionTable![domain];
        state = range.State;
        tape.CurrentSymbol = range.Symbol;
        tape.MoveHeadInDirection(range.HeadDirection);
        Transition<TState, TSymbol> transition = (domain, range);
        OnStepped(new SteppedEventArgs<Transition<TState, TSymbol>>(transition));
    }

    protected override void CleanupComputation()
    {
        tape.Clear();
        transitionTable = null;

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
