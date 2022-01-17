using System.Collections;
using System.Collections.Generic;

namespace TuringMachine.Tests.UnitTests;

internal abstract class TestData : IEnumerable<object[]>
{
    public abstract IEnumerator<object[]> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
