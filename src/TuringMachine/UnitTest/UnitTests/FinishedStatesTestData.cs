using System.Collections.Generic;

namespace TuringMachine.Tests.UnitTests
{
    class FinishedStatesTestData<TState> : TestData
    {
        public override IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { State<TState>.Accept };
            yield return new object[] { State<TState>.Reject };
        }
    }
}
