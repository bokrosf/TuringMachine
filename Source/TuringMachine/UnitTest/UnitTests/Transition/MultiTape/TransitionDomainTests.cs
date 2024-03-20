using System.Collections.Generic;
using TuringMachine.Transition.MultiTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Transition.MultiTape;

public class TransitionDomainTests
{
	[Fact]
	public void Equals_Reflexive_ReturnsTrue()
	{
		var domain = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		bool areEqual = domain.Equals(domain);

		Assert.True(areEqual);
	}

	[Fact]
	public void Equals_Symmetric_ReturnsTrue()
	{
		var state = new State<int>(1);
		var tapeSymbols = new List<Symbol<char>>() { new('a'), new('b') };
		var first = new TransitionDomain<int, char>(state, tapeSymbols);
		var second = new TransitionDomain<int, char>(state, tapeSymbols);

		bool firstEqualsWithSecond = first.Equals(second);
		bool secondEqualsWithFirst = second.Equals(first);

		Assert.True(firstEqualsWithSecond);
		Assert.True(secondEqualsWithFirst);
	}

	[Fact]
	public void Equals_Symmetric_ReturnsFalse()
	{
		var first = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		var second = new TransitionDomain<int, char>(
			new State<int>(2),
			new List<Symbol<char>>() { new('x'), new('y') });

		bool firstEqualsWithSecond = first.Equals(second);
		bool secondEqualsWithFirst = second.Equals(first);

		Assert.False(firstEqualsWithSecond);
		Assert.False(secondEqualsWithFirst);
	}

	[Fact]
	public void Equals_Transitive_ReturnsTrue()
	{
		var state = new State<int>(1);
		var tapeSymbols = new List<Symbol<char>>() { new('a'), new('b') };
		var first = new TransitionDomain<int, char>(state, tapeSymbols);
		var second = new TransitionDomain<int, char>(state, tapeSymbols);
		var third = new TransitionDomain<int, char>(state, tapeSymbols);

		bool firstEqualsWithSecond = first.Equals(second);
		bool secondEqualsWithThird = second.Equals(third);
		bool firstEqualsWithThird = first.Equals(third);

		Assert.True(firstEqualsWithSecond);
		Assert.True(secondEqualsWithThird);
		Assert.True(firstEqualsWithThird);
	}

	[Fact]
	public void Equals_ComparedWithNull_ReturnsFalse()
	{
		var domain = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		bool areEqual = domain.Equals(null);

		Assert.False(areEqual);
	}

	[Fact]
	public void Equals_ComparedWithDifferentType_ReturnsFalse()
	{
		string comparedWith = string.Empty;
		var domain = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		bool areEqual = domain.Equals(comparedWith);

		Assert.False(areEqual);
	}

	[Fact]
	public void GetHashCode_SameValue_ReturnsSameHash()
	{
		var state = new State<int>(1);
		var tapeSymbols = new List<Symbol<char>>() { new('a'), new('b') };
		var first = new TransitionDomain<int, char>(state, tapeSymbols);
		var second = new TransitionDomain<int, char>(state, tapeSymbols);

		int firstHash = first.GetHashCode();
		int secondHash = second.GetHashCode();

		Assert.Equal(firstHash, secondHash);
	}

	[Fact]
	public void EqualityOperator_BothNull_ReturnsTrue()
	{
		TransitionDomain<int, char>? left = null;
		TransitionDomain<int, char>? right = null;

		bool areEqual = left == right;

		Assert.True(areEqual);
	}

	[Fact]
	public void EqualityOperator_OnlyLeftNull_ReturnsFalse()
	{
		TransitionDomain<int, char>? left = null;
		TransitionDomain<int, char>? right = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		bool areEqual = left == right;

		Assert.False(areEqual);
	}

	[Fact]
	public void EqualityOperator_OnlyRightNull_ReturnsFalse()
	{
		TransitionDomain<int, char>? left = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		TransitionDomain<int, char>? right = null;

		bool areEqual = left == right;

		Assert.False(areEqual);
	}

	[Fact]
	public void EqualityOperator_SameValue_ReturnsTrue()
	{
		var state = new State<int>(1);
		var tapeSymbols = new List<Symbol<char>>() { new('a'), new('b') };
		var left = new TransitionDomain<int, char>(state, tapeSymbols);
		var right = new TransitionDomain<int, char>(state, tapeSymbols);

		bool areEqual = left == right;

		Assert.True(areEqual);
	}

	[Fact]
	public void EqualityOperator_DifferentValue_ReturnsFalse()
	{
		var left = new TransitionDomain<int, char>(
			new State<int>(1),
			new List<Symbol<char>>() { new('a'), new('b') });

		var right = new TransitionDomain<int, char>(
			new State<int>(2),
			new List<Symbol<char>>() { new('x'), new('y') });

		bool areEqual = left == right;

		Assert.False(areEqual);
	}
}
