using Moq;
using System;
using System.Threading;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
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
        public void Enforce_NoCancellationRequested_NotCancelled()
        {
            var constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);

            constraint.Enforce(computationStateMock.Object);
        }

        [Fact]
        public void Enforce_CancellationRequested_Cancelled()
        {
            var constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>(MockBehavior.Strict);

            cancellationTokenSource.Cancel();

            Assert.Throws<ComputationCancellationRequestedException>(() => constraint.Enforce(computationStateMock.Object));
        }
    }
}
