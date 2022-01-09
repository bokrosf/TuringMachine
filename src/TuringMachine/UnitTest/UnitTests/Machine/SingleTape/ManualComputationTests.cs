using System;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Machine;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.Constraint;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    public class ManualComputationTests
    {
        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public void Step_WithoutConstraint_SteppedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            var raisedStepped = Assert.Raises<SteppedEventArgs<int, char>>(
                handler => machine.Stepped += handler,
                handler => machine.Stepped -= handler,
                () => machine.Step());

            Assert.Same(machine, raisedStepped.Sender);
            Assert.True(raisedStepped.Arguments.StepCount > 0);
        }

        [Theory]
        [ClassData(typeof(AcceptedInputTestData))]
        public void Step_WithoutConstraint_InputAccepted(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => StepUntilTermination(machine));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Accept, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(RejectedInputTestData))]
        public void Step_WithoutConstraint_InputRejected(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => StepUntilTermination(machine));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(State<int>.Reject, raisedTerminated.Arguments.State);
        }

        [Theory]
        [ClassData(typeof(ExpectedTapeOutputTestData))]
        public void Step_OutputSymbols_SymbolsAsExpected(ExpectedTapeOutputArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => StepUntilTermination(machine));

            Assert.Same(machine, raisedTerminated.Sender);
            Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Step_WithConstraint_EnforcementStopsComputation(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);
            var constraint = new StepLimitConstraint<int, char>(3);

            machine.StartManualComputation(arguments.Input, constraint);
            var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
                handler => machine.ComputationAborted += handler,
                handler => machine.ComputationAborted -= handler,
                () => StepUntilTermination(machine));

            Assert.Null(raisedAborted.Arguments.Exception);
            Assert.NotNull(raisedAborted.Arguments.ConstraintViolation);
            Assert.IsType<StepLimitViolation>(raisedAborted.Arguments.ConstraintViolation);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Step_ManualAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);

            Assert.Throws<InvalidOperationException>(() => machine.StartManualComputation(arguments.Input));
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Abort_NoStepsTaken_Aborted(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
                handler => machine.ComputationAborted += handler,
                handler => machine.ComputationAborted -= handler,
                () => machine.Abort());

            Assert.Same(machine, raisedAborted.Sender);
            Assert.Equal(State<int>.Initial, raisedAborted.Arguments.State);
            Assert.Equal(0, raisedAborted.Arguments.StepCount);
            Assert.IsType<ComputationCancellationRequestedException>(raisedAborted.Arguments.Exception);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Abort_StepTaken_Aborted(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);
            machine.Step();
            var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
                handler => machine.ComputationAborted += handler,
                handler => machine.ComputationAborted -= handler,
                () => machine.Abort());

            Assert.Same(machine, raisedAborted.Sender);
            Assert.NotEqual(State<int>.Initial, raisedAborted.Arguments.State);
            Assert.False(raisedAborted.Arguments.State.IsFinishState);
            Assert.Equal(1, raisedAborted.Arguments.StepCount);
            Assert.IsType<ComputationCancellationRequestedException>(raisedAborted.Arguments.Exception);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Abort_NoComputationStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            Assert.Throws<InvalidOperationException>(() => machine.Abort());
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public async Task Abort_AutomaticAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
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
            Assert.Throws<InvalidOperationException>(() => machine.Abort());

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        private void StepUntilTermination<TState, TSymbol>(SingleTapeMachine<TState, TSymbol> machine)
        {
            while (machine.Step())
            {
                ;
            }
        }
    }
}
