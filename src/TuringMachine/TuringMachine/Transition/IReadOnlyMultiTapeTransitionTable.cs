namespace TuringMachine.Transition
{
    /// <summary>
    /// Defines operations for a readonly multi tape transition table.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public interface IReadOnlyMultiTapeTransitionTable<TState, TValue>
    {
        /// <summary>
        ///  Gets the range of a machine transition at the given tape that belongs to the given transition domain.
        /// </summary>
        /// <param name="tapeId">Identifier of the tape.</param>
        /// <param name="domain">Domain of a multi tape machine transition.</param>
        /// <returns><see cref="TransitionRange{TState, TValue}"/> that belongs to the given transition domain.</returns>
        TransitionRange<TState, TValue> GetTransitionRangeAtTape(int tapeId, TransitionDomain<TState, TValue> domain);
    }
}
