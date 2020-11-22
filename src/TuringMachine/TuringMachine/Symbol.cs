﻿using System.Collections.Generic;

namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine tape symbol.
    /// </summary>
    /// <typeparam name="T">Typeof of the symbolised data.</typeparam>
    public sealed class Symbol<T>
    {
        private const int HashCodeIfValueIsNull = 0;
        
        /// <summary>
        /// Gets the blank symbol tape value.
        /// </summary>
        public static Symbol<T> Blank { get; }

        /// <summary>
        /// Gets the symbolised value.
        /// </summary>
        public T Value { get; }

        static Symbol()
        {
#pragma warning disable CS8604
            // Possible null reference argument.
            // The compiler does not allow to specify nullable generic type parameter where nullability is accepted with both struct and class.
            Blank = new Symbol<T>(default);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Symbol{T}"/> class with the given value.
        /// </summary>
        /// <param name="value">The value to be stored on the tape.</param>
        public Symbol(T value)
        {
#pragma warning disable CS8601 
            // Possible null reference assignment. 
            // The compiler does not allow to specify nullable generic type parameter where nullability is accepted with both struct and class.
            // If 'value' parameter is decorated with [AllowNull] attribute then null vallue can be passed even if the type parameter is non nullable.
            Value = value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

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
            return (left, right) switch
            {
                (null, null) => true,
                (_, null) => false,
                (null, _) => false,
                (Symbol<T> l, _) => l.Equals(right)
            };
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
                return HashCodeIfValueIsNull;
            }
            else
            {
                return Value.GetHashCode();
            }
        }
    }
}
