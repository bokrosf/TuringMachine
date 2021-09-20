using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    public class SingleTapeMachine<TState, TSymbol> :
        IAutomaticComputation<TSymbol>,
        IComputationTracking<TState, TSymbol>
    {
        public event EventHandler<SteppedEventArgs<TState, TSymbol>>? Stepped;
        public event EventHandler<ComputationTerminatedEventArgs<TState, TSymbol>>? ComputationTerminated;
        public event EventHandler<ComputationAbortedEventArgs<TState, TSymbol>>? ComputationAborted;

        private object computationLock;
        private ComputationMode? computationMode;
        private int stepCount;
        private Stopwatch elapsedTime;
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
            elapsedTime = new Stopwatch();
            tape = new Tape<TSymbol>();
            this.transitionTable = transitionTable;
        }

        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input)
        {
            ResetToComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(cancellationToken: null, maxStepCount: null, timeout: null));
        }

        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, CancellationToken cancellationToken)
        {
            ResetToComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(cancellationToken, maxStepCount: null, timeout: null));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxStepCount"/> is less than 1.</exception>
        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, int maxStepCount)
        {
            ThrowIfInvalidMaxStepCount(maxStepCount);
            ResetToComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(cancellationToken: null, maxStepCount: maxStepCount, timeout: null));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="timeout"/> is less than or equal to <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public Task StartComputationAsync(IEnumerable<Symbol<TSymbol>> input, TimeSpan timeout)
        {
            ThrowIfInvalidTimeout(timeout);
            ResetToComputation(ComputationMode.Automatic, input);

            return Task.Run(() => Compute(cancellationToken: null, maxStepCount: null, timeout: timeout));
        }

        public void StartComputation(IEnumerable<Symbol<TSymbol>> input)
        {
            ResetToComputation(ComputationMode.Automatic, input);
            Compute(cancellationToken: null, maxStepCount: null, timeout: null);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxStepCount"/> is less than 1.</exception>
        public void StartComputation(IEnumerable<Symbol<TSymbol>> input, int maxStepCount)
        {
            ThrowIfInvalidMaxStepCount(maxStepCount);
            ResetToComputation(ComputationMode.Automatic, input);
            Compute(cancellationToken: null, maxStepCount: maxStepCount, timeout: null);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="timeout"/> is less than or equal to <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public void StartComputation(IEnumerable<Symbol<TSymbol>> input, TimeSpan timeout)
        {
            ThrowIfInvalidTimeout(timeout);
            ResetToComputation(ComputationMode.Automatic, input);
            Compute(cancellationToken: null, maxStepCount: null, timeout: timeout);
        }

        private void ThrowIfInvalidMaxStepCount(int maxStepCount)
        {
            if (maxStepCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxStepCount), maxStepCount, "Maximum step count must be greater than 0.");
            }
        }

        private void ThrowIfInvalidTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
            {
                string message = $"Timeout must be greater than {nameof(TimeSpan)}.{nameof(TimeSpan.Zero)}.";
                throw new ArgumentOutOfRangeException(nameof(timeout), timeout, message);
            }
        }

        private void ResetToComputation(ComputationMode computationMode, IEnumerable<Symbol<TSymbol>> input)
        {
            lock (computationLock)
            {
                if (this.computationMode.HasValue)
                {
                    throw new InvalidOperationException();
                }

                this.computationMode = computationMode;
                stepCount = 0;
                tape = new Tape<TSymbol>(input);
                configuration = new TransitionDomain<TState, TSymbol>(State<TState>.Initial, tape.CurrentSymbol);
                elapsedTime.Restart();
            }
        }

        private void Compute(CancellationToken? cancellationToken, int? maxStepCount, TimeSpan? timeout)
        {
            try
            {
                do
                {
                    AbortComputationIfRequested(cancellationToken);
                    CheckThresholdConditions(maxStepCount, timeout);
                    TransitToNextState();
                } while (!CanTerminate());
            }
            catch (ComputationAbortedException ex)
            {
                HandleAbortedComputation(ex);
            }

            Terminate();
        }

        private void AbortComputationIfRequested(CancellationToken? cancellationToken)
        {
            if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
            {
                throw new ComputationAbortionRequestedException();
            }
        }

        private void TransitToNextState()
        {
            TransitionDomain<TState, TSymbol> domainBeforeTransition = configuration!;

            try
            {
                TransitionRange<TState, TSymbol> range = transitionTable[configuration!];
                tape.CurrentSymbol = range.Symbol;
                tape.MoveHeadInDirection(range.HeadDirection);
                configuration = (range.State, range.Symbol);
                Transition<TState, TSymbol> transition = (domainBeforeTransition, range);
                ++stepCount;
                OnStepped(new(stepCount, elapsedTime.Elapsed, transition));
            }
            catch (TransitionDomainNotFoundException)
            {
                configuration = (State<TState>.Reject, domainBeforeTransition.Symbol);
            }
        }

        private void CheckThresholdConditions(int? maxStepCount, TimeSpan? timeout)
        {
            if (stepCount > maxStepCount)
            {
                throw new StepLimitReachedException(maxStepCount.Value);
            }

            if (elapsedTime.Elapsed > timeout)
            {
                throw new TimeLimitReachedException(timeout.Value);
            }
        }

        private bool CanTerminate()
        {
            return configuration != null
                && (configuration.State == State<TState>.Accept || configuration.State == State<TState>.Reject);
        }

        private void HandleAbortedComputation(ComputationAbortedException exception)
        {
            elapsedTime.Stop();
            ComputationAbortedEventArgs<TState, TSymbol> eventArgs = new(
                stepCount,
                elapsedTime.Elapsed,
                configuration!.State,
                tape,
                exception);

            lock (computationLock)
            {
                computationMode = null;
            }

            OnComputationAborted(eventArgs);
        }

        private void Terminate()
        {
            elapsedTime.Stop();
            ComputationTerminatedEventArgs<TState, TSymbol> eventArgs = new(stepCount, elapsedTime.Elapsed, configuration!.State, tape);

            lock (computationLock)
            {
                computationMode = null;
            }

            OnComputationTerminated(eventArgs);
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
