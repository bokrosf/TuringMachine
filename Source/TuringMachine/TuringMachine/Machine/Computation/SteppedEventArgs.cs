using System;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Provides data for the event that is raised when a turing machine transitioned from one state to another.
/// </summary>
/// <typeparam name="TTransition">Type of a machine transition.</typeparam>
public class SteppedEventArgs<TTransition> : EventArgs where TTransition : notnull
{
    /// <summary>
    /// The applied transition during the step.
    /// </summary>
    public TTransition Transition { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SteppedEventArgs{TTransition}"/> class with the the specified computation state
    /// and transition.
    /// </summary>
    /// <param name="computationState">State of the computation.</param>
    /// <param name="transition">The applied transition during the step.</param>
    public SteppedEventArgs(TTransition tranition)
    {
        Transition = tranition;
    }
}
