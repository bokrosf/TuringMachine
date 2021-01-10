using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine
{
    /// <summary>
    /// Represents a Turing machine tape that reads and modifies the stored symbols.
    /// </summary>
    /// <typeparam name="T">Type of the symbols' data.</typeparam>
    internal sealed class Tape<T> : IEnumerable<Symbol<T>>
    {
        /// <summary>
        /// Gets or set the symbol that is pointed by head.
        /// </summary>
        public Symbol<T> PointedByHead 
        {
            get => head.Value;
            set => head.Value = value;
        }

        private readonly LinkedList<Symbol<T>> symbols;
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

        /// <summary>
        /// Moves head one step in the given direction and returns the symbol pointed by head after the operation.
        /// </summary>
        /// <param name="direction">Movement direction.</param>
        /// <returns>The symbol pointed by head after the operation.</returns>
        /// <exception cref="ArgumentException">Thrown is the specified direction is part of type <see cref="TapeHeadDirection"/>.</exception>
        public Symbol<T> MoveHeadInDirection(TapeHeadDirection direction) => direction switch
        {
            TapeHeadDirection.Stay => PointedByHead,
            TapeHeadDirection.Left => MoveHeadToTheLeft(),
            TapeHeadDirection.Right => MoveHeadToTheRight(),
            _ => throw new ArgumentException($"{direction} value is not part of type {typeof(TapeHeadDirection).AssemblyQualifiedName}", nameof(direction))
        };

        /// <summary>
        /// Clears symbols and stores only a blank symbol that is pointed by head.
        /// </summary>
        public void Clear()
        {
            symbols.Clear();
            head = new LinkedListNode<Symbol<T>>(Symbol<T>.Blank);
            symbols.AddFirst(head);
        }


        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Tape{T}"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="Tape{T}"</returns>
        public IEnumerator<Symbol<T>> GetEnumerator()
        {
            foreach (var s in symbols)
            {
                yield return s;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Symbol<T> MoveHeadToTheLeft()
        {
            if (head.Previous == null)
            {
                symbols.AddBefore(head, new LinkedListNode<Symbol<T>>(Symbol<T>.Blank));
            }

            head = head.Previous!;

            return PointedByHead;
        }

        private Symbol<T> MoveHeadToTheRight()
        {
            if (head.Next == null)
            {
                symbols.AddAfter(head, new LinkedListNode<Symbol<T>>(Symbol<T>.Blank));
            }

            head = head.Next!;

            return PointedByHead;
        }
    }
}
