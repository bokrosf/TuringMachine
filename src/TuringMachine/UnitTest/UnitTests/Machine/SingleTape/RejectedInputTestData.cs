using System.Collections.Generic;
using System.Linq;
using TuringMachine.Machine;
using TuringMachine.Transition;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    internal class RejectedInputTestData : TestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return GetSingleStepData();
            yield return GetMultipleStepData();
        }

        private object[] GetSingleStepData()
        {
            var input = "a".Select(c => new Symbol<char>(c));
            var transitions = new Transition<int, char>[]
            {
                ((State<int>.Initial, 'b'), (State<int>.Accept, 'b', TapeHeadDirection.Stay))
            };

            var transitionTable = new TransitionTable<int, char>(transitions);

            return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
        }

        private object[] GetMultipleStepData()
        {
            var input = "aabbccaa".Select(c => new Symbol<char>(c));
            var transitions = new Transition<int, char>[]
            {
                ((State<int>.Initial, 'a'), (1, 'x', TapeHeadDirection.Right)),
                ((1, 'a'), (1, 'x', TapeHeadDirection.Right)),
                ((1, 'b'), (1, 'y', TapeHeadDirection.Right)),
                ((1, Symbol<char>.Blank), (State<int>.Accept, 'z', TapeHeadDirection.Stay)),
            };

            var transitionTable = new TransitionTable<int, char>(transitions);

            return new object[] { new StartComputationArguments<int, char>(transitionTable, input) };
        }
    }
}
