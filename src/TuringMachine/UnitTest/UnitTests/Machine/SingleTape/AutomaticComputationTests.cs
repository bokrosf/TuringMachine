﻿using TuringMachine.Machine;
using TuringMachine.Machine.Computation;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape
{
    public class AutomaticComputationTests
    {
        [Theory]
        [ClassData(typeof(AcceptTerminationTestData))]
        public void StartAutomaticComputation_WithoutConstraint_SteppedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedStepped = Assert.Raises<SteppedEventArgs<int, char>>(
                handler => machine.Stepped += handler,
                handler => machine.Stepped -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedStepped.Sender);
            Assert.Equal(State<int>.Accept, raisedStepped.Arguments.Transition.Range.State);
        }

        [Theory]
        [ClassData(typeof(AcceptTerminationTestData))]
        public void StartAutomaticComputation_WithoutConstraint_ComputationTerminatedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedStepped = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputation(arguments.Input));

            Assert.Same(machine, raisedStepped.Sender);
            Assert.Equal(State<int>.Accept, raisedStepped.Arguments.State);
        }
    }
}