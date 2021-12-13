using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
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
        IManualComputation<TState, TSymbol>,
        IComputationTracking<TState, TSymbol>
    {
        public event EventHandler<SteppedEventArgs<TState, TSymbol>>? Stepped;
        public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
        public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

        private object computationLock;
        private object manualComputationLock;
        private ComputationMode? computationMode;
        private ComputationState<TState, TSymbol>? computationState;
        private IComputationConstraint<TState, TSymbol>? constraint;
        private Tape<TSymbol> tape;
        private TransitionTable<TState, TSymbol> transitionTable;

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
            InitializeComputationWithoutConstraint(ComputationMode.Automatic, input);
            return Task.Run(() => Compute());
        }

        public Task StartAutomaticComputationAsync(IEnumerable<Symbol<TSymbol>> input, IComputationConstraint<TState, TSymbol> constraint)
        {
            InitializeComputation(ComputationMode.Automatic, input, constraint);
            return Task.Run(() => Compute());
        }

        public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input)
        {
            InitializeComputationWithoutConstraint(ComputationMode.Automatic, input);
            Compute();
        }

        public void StartAutomaticComputation(IEnumerable<Symbol<TSymbol>> input, IComputationConstraint<TState, TSymbol> constraint)
        {
            InitializeComputation(ComputationMode.Automatic, input, constraint);
            Compute();
        }

        public void StartManualComputation(IEnumerable<Symbol<TSymbol>> input)
        {
            InitializeComputationWithoutConstraint(ComputationMode.Manual, input);
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
                    if (computationMode != ComputationMode.Manual)
                    {
                        throw new InvalidOperationException($"{computationMode?.ToString() ?? "<null>"} computation mode can not be stepped manually.");
                    }
                }

                return PerformStep();
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Computation has not started manually.</exception>
        public void Abort()
        {
            lock (manualComputationLock)
            {
                lock (computationLock)
                {
                    if (computationMode != ComputationMode.Manual)
                    {
                        throw new InvalidOperationException($"{computationMode?.ToString() ?? "<null>"} computation mode can not be aborted manually.");
                    }                    
                }

                try
                {
                    throw new ComputationCancellationRequestedException();
                }
                catch (Exception ex)
                {
                    HandleAbortedComputation(ex);
                }
            }
        }

        private void InitializeComputationWithoutConstraint(ComputationMode computationMode, IEnumerable<Symbol<TSymbol>> input)
        {
            InitializeComputation(computationMode, input, constraint: null);
        }

        private void InitializeComputation(
            ComputationMode computationMode, 
            IEnumerable<Symbol<TSymbol>> input, 
            IComputationConstraint<TState, TSymbol>? constraint)
        {
            lock (computationLock)
            {
                if (this.computationMode.HasValue)
                {
                    throw new InvalidOperationException($"A(n) {this.computationMode} computation is already in progress.");
                }

                this.computationMode = computationMode;                
            }

            this.constraint = constraint;
            tape = new Tape<TSymbol>(input);
            computationState = new ComputationState<TState, TSymbol>(tape.CurrentSymbol);
            computationState.StartDurationWatch();
        }

        private bool PerformStep()
        {
            try
            {
                TransitToNextState();

                if (CanTerminate())
                {
                    Terminate();
                    return false;
                }

                constraint?.Enforce(computationState!.AsReadOnly());

                return true;
            }
            catch (Exception ex)
            {
                HandleAbortedComputation(ex);
                return false;
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
            TransitionDomain<TState, TSymbol> domainBeforeTransition = computationState!.Configuration;

            try
            {
                TransitionRange<TState, TSymbol> range = transitionTable[domainBeforeTransition];
                tape.CurrentSymbol = range.Symbol;
                tape.MoveHeadInDirection(range.HeadDirection);
                computationState.UpdateConfiguration((range.State, tape.CurrentSymbol));
                Transition<TState, TSymbol> transition = (domainBeforeTransition, range);
                OnStepped(new(computationState.AsReadOnly(), transition));
            }
            catch (TransitionDomainNotFoundException)
            {
                computationState.UpdateConfiguration((State<TState>.Reject, domainBeforeTransition.Symbol));
            }            
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

        private void HandleAbortedComputation(Exception exception)
        {
            computationState!.StopDurationWatch();
            ComputationAbortedEventArgs<TState, TSymbol> eventArgs = new(computationState.AsReadOnly(), tape, exception);
            CleanupComputation();
            OnComputationAborted(eventArgs);
        }

        private void Terminate()
        {
            computationState!.StopDurationWatch();
            ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(computationState.AsReadOnly(), tape);
            CleanupComputation();
            OnComputationTerminated(eventArgs);
        }

        private void CleanupComputation()
        {
            computationState = null;
            constraint = null;
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
