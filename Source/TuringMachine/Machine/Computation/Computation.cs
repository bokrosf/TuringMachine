namespace TuringMachine.Machine.Computation;

/// <summary>
/// Represents a computation.
/// </summary>
/// <typeparam name="TConfiguration">Type of the machine's configuration</typeparam>
/// <param name="Mode">Which mode the computation has been started.</param>
/// <param name="Aborted">Whether the computation has been aborted.</param>
public record Computation(ComputationMode Mode, bool Aborted);
