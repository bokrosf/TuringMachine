using System;

namespace TuringMachine.Machine
{
    /// <summary>
    /// Represents the base class for classes that provides data for the event that is raised when a machine's computation state changed.
    /// </summary>
    public abstract class ComputationStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The number of steps have taken since the start of the computation.
        /// </summary>
        public int StepCount { get; }
        
        /// <summary>
        /// The elapsed time since the start of the computation.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComputationStateChangedEventArgs"/> class with the given step count and duration.
        /// </summary>
        /// <param name="stepCount">The number of steps have taken since the start of the computation.</param>
        /// <param name="duration">The elapsed time since the start of the computation.</param>
        protected ComputationStateChangedEventArgs(int stepCount, TimeSpan duration)
        {
            StepCount = stepCount;
            Duration = duration;
        }
    }
}
