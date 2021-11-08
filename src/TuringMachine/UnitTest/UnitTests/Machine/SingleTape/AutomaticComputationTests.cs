using System.Threading.Tasks;
using TuringMachine.Machine;
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
            Assert.True(raisedStepped.Arguments.StepCount > 0);
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

        [Theory]
        [ClassData(typeof(AcceptTerminationTestData))]
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
        [ClassData(typeof(AcceptTerminationTestData))]
        public async Task StartAutomaticComputationAsync_WithoutConstraint_ComputationTerminatedRaised(StartComputationArguments<int, char> arguments)
        {
            var machine = new SingleTapeMachine<int, char>(arguments.TransitionTable);

            var raisedStepped = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
                handler => machine.ComputationTerminated += handler,
                handler => machine.ComputationTerminated -= handler,
                () => machine.StartAutomaticComputationAsync(arguments.Input));

            Assert.Same(machine, raisedStepped.Sender);
            Assert.Equal(State<int>.Accept, raisedStepped.Arguments.State);
        }
    }
}
