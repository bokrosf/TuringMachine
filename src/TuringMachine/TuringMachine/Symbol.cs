using System.Collections.Generic;

namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine tape symbol.
    /// </summary>
    /// <typeparam name="T">Typeof of the symbolised data.</typeparam>
    public sealed class Symbol<T>
    {
        private const int NullValueHashCode = 0;
        
        /// <summary>
        /// Gets the blank symbol tape value.
        /// </summary>
        public static Symbol<T> Blank { get; }

        /// <summary>
        /// Gets the symbolised value.
        /// </summary>
        public T Value { get; }

        static Symbol() => Blank = new Symbol<T>(default!);

        /// <summary>
        /// Initializes a new instance of <see cref="Symbol{T}"/> class with the given value.
        /// </summary>
        /// <param name="value">The value to be stored on the tape.</param>
        public Symbol(T value) => Value = value;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (ReferenceEquals(obj, Blank))
            {
                return true;
            }

            Symbol<T> symbol = (Symbol<T>)obj;

            return (symbol.Value == null && Value == null) || EqualityComparer<T>.Default.Equals(symbol.Value, Value);
        }

        /// <summary>
        /// Determines whether two specified symbols have the same value.
        /// </summary>
        /// <param name="left">The first symbol to compare.</param>
        /// <param name="right">The second symbol to compare.</param>
        /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
        public static bool operator ==(Symbol<T>? left, Symbol<T>? right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            else if (left == null ^ right == null)
            {
                return false;
            }
            else
            {
                return left!.Equals(right);
            }
        }

        /// <summary>
        /// Determines whether two specified symbols have different value.
        /// </summary>
        /// <param name="left">The first symbol to compare.</param>
        /// <param name="right">The second symbol to compare.</param>
        /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
        public static bool operator !=(Symbol<T>? left, Symbol<T>? right) => !(left == right);

        /// <summary>
        /// Returns the hash code for this symbol.
        /// </summary>
        public override int GetHashCode()
        {
            if (ReferenceEquals(Blank, this))
            {
                return base.GetHashCode();
            }
            else if (Value == null)
            {
                return NullValueHashCode;
            }
            else
            {
                return Value.GetHashCode();
            }
        }
    }
}
