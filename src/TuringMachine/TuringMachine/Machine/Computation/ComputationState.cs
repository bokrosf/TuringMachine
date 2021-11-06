using System;
using System.Diagnostics;
using TuringMachine.Transition;

namespace TuringMachine.Machine.Computation
{
    /// <summary>
    /// Represents the state of a computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public class ComputationState<TState, TSymbol> : IComputationState<TState, TSymbol>, IReadOnlyComputationState<TState, TSymbol>
    {
        private readonly Stopwatch durationWatch;
        
        public TransitionDomain<TState, TSymbol> Configuration { get; private set; }
       
        public int StepCount { get; private set; }
        
        public TimeSpan Duration => durationWatch.Elapsed;

        /// <summary>
        /// Initialzes a new instance of <see cref="ComputationState{TState, TSymbol}"/> class to be in <see cref="State{T}.Initial"/> state
        /// and to contain the specified symbol.
        /// </summary>
        public ComputationState(Symbol<TSymbol> symbol)
        {
            Configuration = (State<TState>.Initial, symbol);
            durationWatch = new Stopwatch();
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Configuration is in finished state.</exception>
        public void UpdateConfiguration(TransitionDomain<TState, TSymbol> configuration)
        {
            if (Configuration.State.IsFinishState)
            {
                throw new InvalidOperationException("Configuration can not be updated after it's in finished state.");
            }
            
            Configuration = configuration;
            ++StepCount;
        }

        public void StartDurationWatch() => durationWatch.Start();

        public void StopDurationWatch() => durationWatch.Stop();

        /// <summary>
        /// Returns a read-only wrapper for the current instance.
        /// </summary>
        /// <returns>An object that acts as a read-only wrapper around the current <see cref="ComputationState{TState, TSymbol}"/>.</returns>
        public IReadOnlyComputationState<TState, TSymbol> AsReadOnly() => new ReadOnlyComputationState<TState, TSymbol>(this);
    }
}
