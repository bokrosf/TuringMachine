using System;
using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class TapeTests
    {
        [Fact]
        public void Constructor_Parameterless_Success()
        {
            var tape = new Tape<int>();

            bool isBlank = tape.CurrentSymbol == Symbol<int>.Blank;

            Assert.True(isBlank);
        }

        [Fact]
        public void Enumeration_OneSymbol_BlankOnly()
        {
            IEnumerable<Symbol<int>> symbols = new Tape<int>(Enumerable.Empty<Symbol<int>>());

            bool isOneSymbolOnly = symbols.Count() == 1;
            bool hasBlankSymbol = symbols.Any(s => s == Symbol<int>.Blank);

            Assert.True(isOneSymbolOnly);
            Assert.True(hasBlankSymbol);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Enumeration_OneSymbol_SameSymbolValue(int symbolValue)
        {
            IEnumerable<Symbol<int>> symbols = new Tape<int>(new Symbol<int>[] { new Symbol<int>(symbolValue) });

            bool isOneSymbolOnly = symbols.Count() == 1;
            bool hasSameSymbolValue = symbols.Any(s => s.Value == symbolValue);

            Assert.True(isOneSymbolOnly);
            Assert.True(hasSameSymbolValue);
        }

        [Theory]
        [InlineData(new int[] { 0, 1 })]
        [InlineData(new int[] { 488, 4786, -21, 8934 })]
        [InlineData(new int[] { 438, 0, 48839, 982723, 894, 3728 })]
        public void Enumeration_MultipleSymbol_SameSymbolValue(IEnumerable<int> symbolValues)
        {
            var tape = new Tape<int>(symbolValues.Select(sv => new Symbol<int>(sv)));

            int[] originalSymbolValues = symbolValues.ToArray();
            int[] tapeSymbolValues = tape.Select(s => s.Value).ToArray();

            bool hasSameElementCount = originalSymbolValues.Length== tapeSymbolValues.Length;
            bool elementsAreEqual = true;

            for (int i = 0; i < originalSymbolValues.Length; i++)
            {
                elementsAreEqual &= (originalSymbolValues[i] == tapeSymbolValues[i]);
            }

            Assert.True(hasSameElementCount);
            Assert.True(elementsAreEqual);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 0, 1 })]
        [InlineData(new int[] { 488, 4786, -21, 8934 })]
        [InlineData(new int[] { 438, 0, 48839, 982723, 894, 3728 })]
        public void Clear_SingleCall_OnlyOneBlankSymbolContained(IEnumerable<int> symbolValues)
        {
            var tape = new Tape<int>(symbolValues.Select(sv => new Symbol<int>(sv)));

            tape.Clear();
            bool isOneSymbolOnly = tape.Count() == 1;
            bool hasBlankSymbol = tape.Any(s => s == Symbol<int>.Blank);

            Assert.True(isOneSymbolOnly);
            Assert.True(hasBlankSymbol);
        }

        [Theory]
        [InlineData(1, new int[] { })]
        [InlineData(2, new int[] { 0, 1 })]
        [InlineData(5, new int[] { 488, 4786, -21, 8934 })]
        [InlineData(100, new int[] { 438, 0, 48839, 982723, 894, 3728 })]
        public void Clear_MultipleCall_OnlyOneBlankSymbolContained(int callCount, IEnumerable<int> symbolValues)
        {
            if (callCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(callCount), "Call count can not be less then one.");
            }

            var tape = new Tape<int>(symbolValues.Select(sv => new Symbol<int>(sv)));

            for (int i = 0; i < callCount; i++)
            {
                tape.Clear();
            }

            bool isOneSymbolOnly = tape.Count() == 1;
            bool hasBlankSymbol = tape.Any(s => s == Symbol<int>.Blank);

            Assert.True(isOneSymbolOnly);
            Assert.True(hasBlankSymbol);
        }

        [Fact]
        public void MoveHeadInDirection_Stay_MatchesOriginalSymbol()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 2).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            Symbol<int> originalSymbol = tape.MoveHeadInDirection(TapeHeadDirection.Stay);

            Assert.Equal(symbols[0], originalSymbol);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheLeft_MatchesLeftSymbol()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 2).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            Symbol<int> previousSymbol = tape.CurrentSymbol;
            tape.MoveHeadInDirection(TapeHeadDirection.Right);
            Symbol<int> leftSymbol = tape.MoveHeadInDirection(TapeHeadDirection.Left);

            Assert.Equal(previousSymbol, leftSymbol);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheRight_MatchesRightSymbol()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 2).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            Symbol<int> rightSymbol = tape.MoveHeadInDirection(TapeHeadDirection.Right);

            Assert.Equal(symbols[1], rightSymbol);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheLeft_MatchesAllTillTheLast()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 10).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            for (int i = 0; i < symbols.Length - 1; i++)
            {
                tape.MoveHeadInDirection(TapeHeadDirection.Right);
            }

            bool allSymbolsMatched = true;

            for (int i = symbols.Length - 1; i >= 0; i--)
            {
                allSymbolsMatched &= symbols[i] == tape.CurrentSymbol;
                tape.MoveHeadInDirection(TapeHeadDirection.Left);
            }

            Assert.True(allSymbolsMatched);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheRight_MatchesAllTillTheLast()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 10).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            bool allSymbolsMatched = true;

            for (int i = 0; i < symbols.Length; i++)
            {
                allSymbolsMatched &= symbols[i] == tape.CurrentSymbol;
                tape.MoveHeadInDirection(TapeHeadDirection.Right);
            }

            Assert.True(allSymbolsMatched);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheLeft_BlankSymbolCreatedAfterLast()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 10).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            tape.MoveHeadInDirection(TapeHeadDirection.Left);
            
            Assert.Equal(Symbol<int>.Blank, tape.CurrentSymbol);
        }

        [Fact]
        public void MoveHeadInDirection_ToTheRight_BlankSymbolCreatedAfterLast()
        {
            Symbol<int>[] symbols = Enumerable.Range(1, 10).Select(i => new Symbol<int>(i)).ToArray();
            var tape = new Tape<int>(symbols);

            for (int i = 0; i < symbols.Length; i++)
            {
                tape.MoveHeadInDirection(TapeHeadDirection.Right);
            }

            Assert.Equal(Symbol<int>.Blank, tape.CurrentSymbol);
        }
    }
}
