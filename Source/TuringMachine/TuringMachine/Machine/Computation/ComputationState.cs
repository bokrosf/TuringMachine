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
    /// <exception cref="InvalidOperationException">Configuration is in finished state.</exception>
    public void UpdateConfiguration(TConfiguration configuration)
    {
        if (IsFinishState(Configuration))
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
    /// <returns>An object that acts as a read-only wrapper around the current <see cref="ComputationState{TConfiguration}"/>.</returns>
    public IReadOnlyComputationState<TConfiguration> AsReadOnly() => new ReadOnlyComputationState<TConfiguration>(this);

    /// <summary>
    /// Returns whether the configuration is in finish state.
    /// </summary>
    /// <param name="configuration">Configuration of the machine.</param>
    /// <returns>true if the configuration is in finished state; otherwise, false.</returns>
    protected abstract bool IsFinishState(TConfiguration configuration);
}
