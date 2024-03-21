namespace TuringMachine.Machine.Computation;

/// <summary>
/// Specifies computation execution modes.
/// </summary>
public enum ComputationMode
{
    /// <summary>
    /// The computation executed automatically without client interference.
    /// </summary>
    Automatic,

    /// <summary>
    /// The computation executed manually by the client step by step.
    /// </summary>
    Manual
}
