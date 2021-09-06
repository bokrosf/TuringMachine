using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TuringMachine.Transition;
using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class TransitionCollectionValidatorTests
    {
        TransitionCollectionValidator<string, int> validator;

        public TransitionCollectionValidatorTests()
        {
            validator = new TransitionCollectionValidator<string, int>();
        }

        [Fact]
        public void Validate_ValidCollection()
        {
            var transitions = new Transition<string, int>[]
            {
                ((State<string>.Initial, new Symbol<int>(1)), ("q1", 2, TapeHeadDirection.Right)),
                (("q1", 2), ("q1", 3, TapeHeadDirection.Stay)),
                (("q1", 3), (State<string>.Accept, new Symbol<int>(4), TapeHeadDirection.Stay)),
            };

            validator.Validate(transitions);
        }

        [Fact]
        public void Validate_EmptyCollection_NotValid()
        {
            var transitions = Enumerable.Empty<Transition<string, int>>();

            Assert.Throws<NoTransitionProvidedException>(() => validator.Validate(transitions));
        }

        [Fact]
        public void Validate_DuplicateTransition_NotValid()
        {
            var duplicateTransition = (("q0", 1), ("q1", 2, TapeHeadDirection.Right));

            var transitions = new Transition<string, int>[]
            {
                duplicateTransition,
                (("q1", 2), ("q2", 3, TapeHeadDirection.Stay)),
                duplicateTransition
            };

            Assert.Throws<DuplicateTransitionException>(() => validator.Validate(transitions));
        }

        [Fact]
        public void Validate_NonDeterministic_NotValid()
        {
            var domain = ("q0", 1);

            var transitions = new Transition<string, int>[]
            {
                (domain, ("q1", 2, TapeHeadDirection.Right)),
                (domain, ("q2", 4, TapeHeadDirection.Stay)),
            };

            Assert.Throws<NonDeterministicTransitionException>(() => validator.Validate(transitions));
        }

        [Theory]
        [ClassData(typeof(InvalidDomainStateTestData<string>))]
        public void Validate_InvalidDomainState_NotValid(State<string> invalidDomainState)
        {
            var transition = new Transition<string, int>(
                (invalidDomainState, new Symbol<int>(2)),
                ("q1", 2, TapeHeadDirection.Right));
            
            var transitions = new Transition<string, int>[]
            {
                ((State<string>.Initial, 1), ("q1", 3, TapeHeadDirection.Left)),
                transition,
                (("qEnd", 2), (State<string>.Accept, 3, TapeHeadDirection.Left)),
            };

            Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
        }

        [Theory]
        [ClassData(typeof(InvalidRangeStateTestData<string>))]
        public void Validate_InvalidRangeState_NotValid(State<string> invalidRangeState)
        {
            var transition = new Transition<string, int>(
                ("q1", 1),
                (invalidRangeState, new Symbol<int>(2), TapeHeadDirection.Right));

            var transitions = new Transition<string, int>[]
            {
                ((State<string>.Initial, 1), ("q1", 3, TapeHeadDirection.Left)),
                transition,
                (("qEnd", 2), (State<string>.Accept, 3, TapeHeadDirection.Left)),
            };

            Assert.Throws<InvalidStateInTransitionException>(() => validator.Validate(transitions));
        }
    }

    class InvalidDomainStateTestData<TState> : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { State<TState>.Accept };
            yield return new object[] { State<TState>.Failure };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class InvalidRangeStateTestData<TState> : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { State<TState>.Initial };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
