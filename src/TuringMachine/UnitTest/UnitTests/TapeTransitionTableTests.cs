using TuringMachine.Transition;
using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class TapeTransitionTableTests
    {
        private readonly TapeTransitionTable<char, int> transitionTable;

        public TapeTransitionTableTests()
        {
            transitionTable = new TapeTransitionTable<char, int>();
        }
        
        [Fact]
        public void Indexer_Get_NoItemExists()
        {
            TransitionDomain<char, int> domain = ('a', 1);

            Assert.Throws<TransitionDomainNotFoundException>(() => transitionTable[domain]);
        }

        [Fact]
        public void Indexer_Get_ExistingItem()
        {
            Transition<char, int> transition = (('a', 1), ('b', 2, TapeHeadDirection.Right));
            transitionTable[transition.Domain] = transition.Range;

            var registeredRange = transitionTable[transition.Domain];

            Assert.Equal(transition.Range, registeredRange);
        }

        [Fact]
        public void Indexer_Set_ModifyItem()
        {
            TransitionDomain<char, int> domain = ('a', 1);
            TransitionRange<char, int> originalRange = ('b', 2, TapeHeadDirection.Right);
            TransitionRange<char, int> newRange = ('c', 3, TapeHeadDirection.Left);
            transitionTable[domain] = originalRange;
            
            transitionTable[domain] = newRange;
            TransitionRange<char, int> registeredRange = transitionTable[domain];

            Assert.Equal(newRange, registeredRange);
        }

        [Fact]
        public void Indexer_Set_OnlyTheSpecifiedItemModified()
        {
            TransitionDomain<char, int> firstDomain = ('a', 1);
            TransitionDomain<char, int> secondDomain = ('b', 2);
            TransitionRange<char, int> firstRange = ('a', 1, TapeHeadDirection.Right);
            TransitionRange<char, int> secondRange = ('b', 2, TapeHeadDirection.Right);
            TransitionRange<char, int> newFirstRange = ('c', 3, TapeHeadDirection.Stay);
            transitionTable[firstDomain] = firstRange;
            transitionTable[secondDomain] = secondRange;
            
            transitionTable[firstDomain] = newFirstRange;
            TransitionRange<char, int> registeredFirstRange = transitionTable[firstDomain];
            TransitionRange<char, int> registeredSecondRange = transitionTable[secondDomain];

            Assert.Equal(newFirstRange, registeredFirstRange);
            Assert.Equal(secondRange, registeredSecondRange);
        }
    }
}
