using System;
using System.Threading;
using TuringMachine.Machine;
using TuringMachine.Machine.ComputationConstraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.ComputationConstraint
{
    public class CancellationConstraintTests : IDisposable
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        public CancellationConstraintTests()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            cancellationTokenSource.Dispose();
        }

        [Fact]
        public void Ensure_NoCancellationRequested_NotCancelled()
        {
            var constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));
            constraint.Enforce(computationState);
        }

        [Theory]
        [ClassData(typeof(FinishedStatesTestData<int>))]
        public void Ensure_FinishedAndCancellationRequested_NotCancelled(State<int> finishedState)
        {
            var constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));
            computationState.UpdateConfiguration((finishedState, 'G'));

            cancellationTokenSource.Cancel();

            constraint.Enforce(computationState);
        }

        [Fact]
        public void Ensure_UnfinishedAndCancellationRequested_Cancelled()
        {
            var constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            var computationState = new ComputationState<int, char>(new Symbol<char>('F'));

            cancellationTokenSource.Cancel();

            Assert.Throws<ComputationCancellationRequestedException>(() => constraint.Enforce(computationState));
        }
    }
}
