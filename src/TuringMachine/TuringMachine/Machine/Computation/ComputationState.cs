using System;
using System.Diagnostics;

namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents the state of a computation.
/// </summary>
/// <typeparam name="TConfiguration">Type of the machine's configuration.</typeparam>
public abstract class ComputationState<TConfiguration> : IReadOnlyComputationState<TConfiguration>
{
    public int StepCount { get; protected set; }
    public TimeSpan Duration => durationWatch.Elapsed;
    public abstract TConfiguration Configuration { get; protected set; }

    private readonly Stopwatch durationWatch;

    /// <summary>
    /// Initializes a new instance of <see cref="ComputationState{TConfiguration}"/> class.
    /// </summary>
    protected ComputationState() => durationWatch = new Stopwatch();

    /// <summary>
    /// Updates the configuration to the specified one.
    /// </summary>
    /// <param name="configuration">Configuration of the matchine.</param>
    public abstract void UpdateConfiguration(TConfiguration configuration);

    public void StartDurationWatch() => durationWatch.Start();

    public void StopDurationWatch() => durationWatch.Stop();

    /// <summary>
    /// Returns a read-only wrapper for the current instance.
    /// </summary>
    /// <returns>An object that acts as a read-only wrapper around the current <see cref="ComputationState{TConfiguration}"/>.</returns>
    public abstract IReadOnlyComputationState<TConfiguration> AsReadOnly();
}
