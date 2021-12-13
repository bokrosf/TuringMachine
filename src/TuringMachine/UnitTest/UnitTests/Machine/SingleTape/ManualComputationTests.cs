using System;
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

            Assert.True(raisedAborted.Arguments.Exception is ComputationAbortedException);
        }

        [Theory]
        [ClassData(typeof(InfiniteComputationTestData))]
        public void Step_ManualAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            machine.StartManualComputation(arguments.Input);

            Assert.Throws<InvalidOperationException>(() => machine.StartManualComputation(arguments.Input));
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
