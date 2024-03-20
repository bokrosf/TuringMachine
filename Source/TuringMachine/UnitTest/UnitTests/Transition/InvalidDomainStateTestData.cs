using System.Collections;
using System.Collections.Generic;

namespace TuringMachine.Tests.UnitTests.Transition;

class InvalidDomainStateTestData<TState> : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { State<TState>.Accept };
		yield return new object[] { State<TState>.Reject };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}