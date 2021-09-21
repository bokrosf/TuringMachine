using System;
using TuringMachine.Transition;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents the state of a computation.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TSymbol">Type of the symbolised data.</typeparam>
    public record ComputationState<TState, TSymbol>(TransitionDomain<TState, TSymbol> Configuration, int StepCount, TimeSpan ElapsedTime);
}
