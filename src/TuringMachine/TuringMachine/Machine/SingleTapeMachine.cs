using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    public class SingleTapeMachine<TState, TSymbol> : IComputationTracking<TState, TSymbol>
    {
        public event EventHandler<SteppedEventArgs<TState, TSymbol>>? Stepped;
        public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;

        private object computationLock;
        private ComputationMode? computationMode;
        private int stepCount;
        private Tape<TSymbol> tape;
        private TransitionDomain<TState, TSymbol>? configuration;
        private TransitionTable<TState, TSymbol> transitionTable;

        /// <summary>
        /// Initializes a new instance of <see cref="SingleTapeMachine{TState, TSymbol}"/> class with the given transition table.
        /// </summary>
        /// <param name="transitionTable">Table that contains the performable transitions.</param>
        public SingleTapeMachine(TransitionTable<TState, TSymbol> transitionTable)
        {
            computationLock = new object();
            tape = new Tape<TSymbol>();
            this.transitionTable = transitionTable;
        }
    }
}
