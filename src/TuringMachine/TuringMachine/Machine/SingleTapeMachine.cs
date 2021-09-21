using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TuringMachine.Machine.ComputationConstraint;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents a single-tape turing machine.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class SingleTapeMachine<TState, TSymbol> :
        IAutomaticComputation<TState, TSymbol>,
        IComputationTracking<TState, TSymbol>
    {
        public event EventHandler<SteppedEventArgs<TState, TSymbol>>? Stepped;
        public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
        public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

        private object computationLock;
        private ComputationMode? computationMode;
        private ComputationState<TState, TSymbol>? computationState;
        private Stopwatch elapsedTimeWatch;
        private Tape<TSymbol> tape;
        private TransitionTable<TState, TSymbol> transitionTable;

        /// <summary>
        /// Initializes a new instance of <see cref="SingleTapeMachine{TState, TSymbol}"/> class with the given transition table.
        /// </summary>
        /// <param name="transitionTable">Table that contains the performable transitions.</param>
        public SingleTapeMachine(TransitionTable<TState, TSymbol> transitionTable)
        {
            computationLock = new object();
            elapsedTimeWatch = new Stopwatch();
            tape = new Tape<TSymbol>();
            this.transitionTable = transitionTable;
        }

        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input)
        {
            InitializeComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(constraint: null));
        }

        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, ComputationConstraint<TState, TSymbol> constraint)
        {
            InitializeComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(constraint));
        }

        public void StartComputation(IEnumerable<Symbol<TSymbol>> input)
        {
            InitializeComputation(ComputationMode.Automatic, input);
            Compute(constraint: null);
        }

        public void StartComputation(IEnumerable<Symbol<TSymbol>> input, ComputationConstraint<TState, TSymbol> constraint)
        {
            InitializeComputation(ComputationMode.Automatic, input);
            Compute(constraint);
        }

        private void InitializeComputation(ComputationMode computationMode, IEnumerable<Symbol<TSymbol>> input)
        {
            lock (computationLock)
            {
                if (this.computationMode.HasValue)
                {
                    throw new InvalidOperationException($"A(n) {this.computationMode} computation is already in progress.");
                }

                this.computationMode = computationMode;
                tape = new Tape<TSymbol>(input);
                TransitionDomain<TState, TSymbol> initialConfiguration = (State<TState>.Initial, tape.CurrentSymbol);
                computationState = new ComputationState<TState, TSymbol>(initialConfiguration, 0, TimeSpan.Zero);
                elapsedTimeWatch.Restart();
            }
        }

        private void Compute(ComputationConstraint<TState, TSymbol>? constraint)
        {
            try
            {
                do
                {
                    TransitToNextState();
                    constraint?.Enforce(computationState!.AsReadOnly());
                } while (!CanTerminate());
            }
            catch (ComputationAbortedException ex)
            {
                HandleAbortedComputation(ex);
            }

            Terminate();
        }

        private void TransitToNextState()
        {
            TransitionDomain<TState, TSymbol> domainBeforeTransition = computationState!.Configuration;

            try
            {
                TransitionRange<TState, TSymbol> range = transitionTable[domainBeforeTransition];
                tape.CurrentSymbol = range.Symbol;
                tape.MoveHeadInDirection(range.HeadDirection);
                UpdateComputationState((range.State, range.Symbol));
                Transition<TState, TSymbol> transition = (domainBeforeTransition, range);
                OnStepped(new(computationState.StepCount, elapsedTimeWatch.Elapsed, transition));
            }
            catch (TransitionDomainNotFoundException)
            {
                UpdateComputationState((State<TState>.Reject, domainBeforeTransition.Symbol));
            }            
        }

        private void UpdateComputationState(TransitionDomain<TState, TSymbol> newConfiguration)
        {
            computationState!.Configuration = newConfiguration;
            ++computationState.StepCount;
            computationState.ElapsedTime = elapsedTimeWatch.Elapsed;
        }

        private bool CanTerminate()
        {
            if (computationState == null)
            {
                return false;
            }

            State<TState> state = computationState.Configuration.State;

            return state == State<TState>.Accept || state == State<TState>.Reject;
        }

        private void HandleAbortedComputation(ComputationAbortedException exception)
        {
            elapsedTimeWatch.Stop();
            computationState!.ElapsedTime = elapsedTimeWatch.Elapsed;
            ComputationAbortedEventArgs<TState, TSymbol> eventArgs = new(computationState!.AsReadOnly(), tape, exception);
            CleanupComputation();
            OnComputationAborted(eventArgs);
        }

        private void Terminate()
        {
            elapsedTimeWatch.Stop();
            computationState!.ElapsedTime = elapsedTimeWatch.Elapsed;
            ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(computationState!.AsReadOnly(), tape);
            CleanupComputation();
            OnComputationTerminated(eventArgs);
        }

        private void CleanupComputation()
        {
            computationState = null;
            tape.Clear();

            lock (computationLock)
            {
                computationMode = null;
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
}
