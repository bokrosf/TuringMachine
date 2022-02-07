namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents a read-only computation state with the machine's configuration.
/// </summary>
/// <typeparam name="TConfiguration">Type of the machine's configuration.</typeparam>
public interface IReadOnlyComputationState<TConfiguration> : IReadOnlyComputationState
{
    /// <summary>
    /// Configuration of the machine.
    /// </summary>
    public TConfiguration Configuration { get; }
}