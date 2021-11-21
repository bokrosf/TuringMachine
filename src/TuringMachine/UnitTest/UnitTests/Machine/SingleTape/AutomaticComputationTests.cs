using System.Threading.Tasks;
using TuringMachine.Machine;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    public class AutomaticComputationTests
    {
        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public void StartAutomaticComputation_WithoutConstraint_SteppedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedStepped = Assert.Raises<SteppedEventArgs<int, char>>(
                handler => machine.Stepped += handler,
                handler => machine.Stepped -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedStepped.Sender);
            Assert.True(raisedStepped.Arguments.StepCount > 0);
            Assert.Equal(State<int>.Accept, raisedStepped.Arguments.Transition.Range.State);
        }

        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public void StartAutomaticComputation_WithoutConstraint_InputAccepted(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Accept, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(RejectedInputTestData))]
        public void StartAutomaticComputation_WithoutConstraint_InputRejected(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Reject, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(ExpectedTapeOutputTestData))]
        public void StartAutomaticComputation_OutputSymbols_SymbolsAsExpected(ExpectedTapeOutputArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void StartAutomaticComputation_WithConstraint_EnforcementStopsComputation(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);
            var constraint = new StepLimitConstraint<int, char>(3);

            var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
                handler => machine.ComputationAborted += handler,
                handler => machine.ComputationAborted -= handler,
                () => machine.StartAutomaticComputation(arguments.Input, constraint));

            Assert.True(raisedAborted.Arguments.Exception is ComputationAbortedException);
        }

        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public async Task StartAutomaticComputationAsync_WithoutConstraint_SteppedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedStepped = await Assert.RaisesAsync<SteppedEventArgs<int, char>>(
                handler => machine.Stepped += handler,
                handler => machine.Stepped -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input));

            Assert.Same(machine, raisedStepped.Sender);
            Assert.True(raisedStepped.Arguments.StepCount > 0);
            Assert.Equal(State<int>.Accept, raisedStepped.Arguments.Transition.Range.State);
        }

        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public async Task StartAutomaticComputationAsync_WithoutConstraint_InputAccepted(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Accept, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(RejectedInputTestData))]
        public async Task StartAutomaticComputationAsync_WithoutConstraint_InputRejected(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Reject, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(ExpectedTapeOutputTestData))]
        public async Task StartAutomaticComputationAsync_OutputSymbols_SymbolsAsExpected(ExpectedTapeOutputArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public async Task StartAutomaticComputationAsync_WithConstraint_EnforcementStopsComputation(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);
            var constraint = new StepLimitConstraint<int, char>(3);

            var raisedAborted = await Assert.RaisesAsync<ComputationAbortedEventArgs<int, char>>(
                handler => machine.ComputationAborted += handler,
                handler => machine.ComputationAborted -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input, constraint));

            Assert.True(raisedAborted.Arguments.Exception is ComputationAbortedException);
        }
    }
}
