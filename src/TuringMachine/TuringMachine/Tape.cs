using System.Collections.Generic;
using System.Linq;

namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine tape that reads and modifies the stored symbols.
    /// </summary>
    /// <typeparam name="T">Type of the symbols' data.</typeparam>
    internal sealed class Tape<T>
    {
        public Symbol<T> CurrentSymbol 
        {
            get => head.Value;
            set => head.Value = value;
        }

        private LinkedList<Symbol<T>> symbols;
        private LinkedListNode<Symbol<T>> head;

        /// <summary>
        /// Initializes a new instance of <see cref="Tape{T}"/> class that only contains a single blank symbol that is pointed by head.
        /// </summary>
        public Tape()
            : this(Enumerable.Empty<Symbol<T>>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Tape{T}"/> class with the given symbols and the first symbol is pointed by head.
        /// </summary>
        /// <param name="symbols">Symbols to be stored on the tape.</param>
        public Tape(IEnumerable<Symbol<T>> symbols)
        {
            this.symbols = new LinkedList<Symbol<T>>(symbols);
            head = this.symbols.First ?? new LinkedListNode<Symbol<T>>(Symbol<T>.Blank);

            if (!this.symbols.Any())
            {
                this.symbols.AddFirst(head);
            }
        }
    }
}
