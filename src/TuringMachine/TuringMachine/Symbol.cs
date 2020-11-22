namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine tape symbol.
    /// </summary>
    /// <typeparam name="T">Typeof of the symbolised data.</typeparam>
    public class Symbol<T>
    {
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
    }
}
