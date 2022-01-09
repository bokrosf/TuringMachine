using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
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

            Assert.Null(raisedAborted.Arguments.Exception);
            Assert.NotNull(raisedAborted.Arguments.ConstraintViolation);
            Assert.IsType<StepLimitViolation>(raisedAborted.Arguments.ConstraintViolation);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public async Task StartAutomaticComputation_AutomaticAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationConstraint<int, char> constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            Task firstStepSynchronizationTask = new Task(() => { });

            void HandleMachineStepped(object? sender, SteppedEventArgs<int, char> e)
            {
                machine.Stepped -= HandleMachineStepped;
                firstStepSynchronizationTask.Start();
            }

            machine.Stepped += HandleMachineStepped;

            // Thread creation needed because using Task.Run() can cause the infinite computation run forever because scheduling.
            Thread computationThread = new Thread(() => machine.StartAutomaticComputation(arguments.Input, constraint));
            computationThread.Priority = ThreadPriority.Lowest;
            computationThread.Start();
            await firstStepSynchronizationTask;
            Assert.Throws<InvalidOperationException>(() => machine.StartAutomaticComputation(arguments.Input));

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
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

            Assert.Null(raisedAborted.Arguments.Exception);
            Assert.NotNull(raisedAborted.Arguments.ConstraintViolation);
            Assert.IsType<StepLimitViolation>(raisedAborted.Arguments.ConstraintViolation);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public async Task StartAutomaticComputationAsync_AutomaticAlreadyStarted_ThrowsException(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var computationStateMock = new Mock<IReadOnlyComputationState<int, char>>();
            computationStateMock.Setup(cs => cs.Configuration).Returns((1, 'a'));
            CancellationConstraint<int, char> constraint = new CancellationConstraint<int, char>(cancellationTokenSource.Token);
            Task firstStepSynchronizationTask = new Task(() => { });

            void HandleMachineStepped(object? sender, SteppedEventArgs<int, char> e)
            {
                machine.Stepped -= HandleMachineStepped;
                firstStepSynchronizationTask.Start();
            }

            machine.Stepped += HandleMachineStepped;

            // Thread creation needed because using Task.Run() can cause the infinite computation run forever because scheduling.
            Thread computationThread = new Thread(() => machine.StartAutomaticComputation(arguments.Input, constraint));
            computationThread.Priority = ThreadPriority.Lowest;
            computationThread.Start();
            await firstStepSynchronizationTask;
            await Assert.ThrowsAsync<InvalidOperationException>(() => machine.StartAutomaticComputationAsync(arguments.Input));

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public async Task StartAutomaticComputationAsync_ManualAlreadyStarted_ThrowsException(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);

            await Assert.ThrowsAsync<InvalidOperationException>(() => machine.StartAutomaticComputationAsync(arguments.Input));
        }
    }
}
