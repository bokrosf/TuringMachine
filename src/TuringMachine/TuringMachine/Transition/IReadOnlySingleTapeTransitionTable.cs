namespace TuringMachine.Transition
{
    /// <summary>
    /// Defines operations for a readonly single tape transition table.
    /// </summary>
    /// <typeparam name="TState">Type of the machine's state.</typeparam>
    /// <typeparam name="TValue">Type of the symbolised data.</typeparam>
    public interface IReadOnlySingleTapeTransitionTable<TState, TValue>
    {
        /// <summary>
        /// Gets the range of a machine transition that belongs to the given transition domain.
        /// </summary>
        /// <param name="domain">Domain of a single tape machine transition.</param>
        /// <returns><see cref="TransitionRange{TState, TValue}"/> that belongs to the given transition domain.</returns>
        TransitionRange<TState, TValue> GetRange(TransitionDomain<TState, TValue> domain);
    }
}
