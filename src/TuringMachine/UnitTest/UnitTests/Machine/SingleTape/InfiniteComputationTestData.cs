using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    internal class InfiniteComputationTestData : TestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return GetTestData();
        }

        private object[] GetTestData()
        {
            var input = "a".Select(c => new Symbol<char>(c));
            var transitions = new Transition<int, char>[]
            {
                ((State<int>.Initial, input.First()), (1, input.First(), TapeHeadDirection.Stay)),
                ((1, input.First()), (1, input.First(), TapeHeadDirection.Stay)),
                ((2, input.First()), (State<int>.Accept, input.First(), TapeHeadDirection.Stay))
            };

            var transitionTable = new TransitionTable<int, char>(transitions);

            return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
        }
    }
}
