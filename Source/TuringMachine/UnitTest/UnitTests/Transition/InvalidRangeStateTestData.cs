using System.Collections;
using System.Collections.Generic;

namespace TuringMachine.Tests.UnitTests.Transition;

class InvalidRangeStateTestData<TState> : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { State<TState>.Initial };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

