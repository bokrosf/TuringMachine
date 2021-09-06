using System.Collections.Generic;
using System.Linq;

namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine state.
    /// </summary>
    /// <typeparam name="T">Type of the state.</typeparam>
    public class State<T>
    {
        private const int NullValueHashCode = 0;
        private const int InitialHashCode = 100003;
        private const int AcceptHashCode = 500009;
        private const int FailureHashCode = 900007;

        /// <summary>
        /// Initial state of the machine, when computation has not started.
        /// </summary>
        public static State<T> Initial { get; }
        
        /// <summary>
        /// Accept state of the machine, when computation terminated normally.
        /// </summary>
        public static State<T> Accept { get; }

        /// <summary>
        /// Failure state of the machine, when there is no valid transition from the current state of the machine.
        /// </summary>
        public static State<T> Failure { get; }

        /// <summary>
        /// Gets the value that represents the state.
        /// </summary>
        public T Value { get; }

        static State()
        {
            Initial = new State<T>(default!);
            Accept = new State<T>(default!);
            Failure = new State<T>(default!);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="State{T}"/> class with the given value.
        /// </summary>
        /// <param name="value">The value that represents a state.</param>
        public State(T value) => Value = value;

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            State<T> other = (State<T>)obj;

            if (AreBothSpecialState(other, this))
            {
                return true;
            }

            return !IsAnyOfThemSpecialState(other, this) && EqualityComparer<T>.Default.Equals(other.Value, Value);
        }

        /// <summary>
        /// Returns the hash code for this state.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this switch
            {
                _ when ReferenceEquals(this, Initial) => InitialHashCode,
                _ when ReferenceEquals(this, Accept) => AcceptHashCode,
                _ when ReferenceEquals(this, Failure) => FailureHashCode,
                _ => Value?.GetHashCode() ?? NullValueHashCode
            };
        }

        /// <summary>
        /// Determines whether two specified states have the same value.
        /// </summary>
        /// <param name="left">The first state to compare.</param>
        /// <param name="right">The second state to compare.</param>
        /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
        public static bool operator==(State<T>? left, State<T>? right)
        {
            return (left, right) switch
            {
                (null, null) => true,
                (null, _) => false,
                (_, _) => left.Equals(right)
            };
        }

        /// <summary>
        /// Determines whether two specified states have different value.
        /// </summary>
        /// <param name="left">The first state to compare.</param>
        /// <param name="right">The second state to compare.</param>
        /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
        public static bool operator !=(State<T>? left, State<T>? right) => !(left == right);

        /// <summary>
        /// Converts the value of this instance to a <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// <see cref="string"/> whose value is the same as the string representation of <see cref="Value"/>. If it's a special state then
        /// it returns the special state's name.
        /// </returns>
        public override string ToString()
        {
            return this switch
            {
                _ when ReferenceEquals(this, Initial) => nameof(Initial),
                _ when ReferenceEquals(this, Accept) => nameof(Accept),
                _ when ReferenceEquals(this, Failure) => nameof(Failure),
                _ => $"{Value}"
            };
        }

        private bool AreBothSpecialState(object first, object second)
        {
            return GetSpecialStates().Any(special => ReferenceEquals(first, special) && ReferenceEquals(second, special));
        }

        private bool IsAnyOfThemSpecialState(object first, object second)
        {
            return GetSpecialStates().Any(special => ReferenceEquals(first, special) || ReferenceEquals(second, special));
        }

        private IEnumerable<State<T>> GetSpecialStates()
        {
            yield return Initial;
            yield return Accept;
            yield return Failure;
        }
    }
}
