using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.Computation.MultiTape;
using TuringMachine.Machine.MultiTape;
using TuringMachine.Transition.MultiTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.MultiTape;

public class AutomaticComputationTests
{
	[Theory]
	[ClassData(typeof(AcceptedInputTestData))]
	public async Task StartAutomaticAsync_SteppedRaised(StartComputationArguments<int, char> arguments)
	{
		var machine = new Machine<int, char>();

		var raisedStepped = await Assert.RaisesAsync<SteppedEventArgs<Transition<int, char>>>(
			handler => machine.Stepped += handler,
			handler => machine.Stepped -= handler,
			() => machine.StartAutomaticAsync(new ComputationRequest<int, char>(arguments.Input, arguments.TransitionTable)));

		Assert.Same(machine, raisedStepped.Sender);
		Assert.Equal(State<int>.Accept, raisedStepped.Arguments.Transition.State.Range);
	}

	[Theory]
	[ClassData(typeof(AcceptedInputTestData))]
	public async Task StartAutomaticAsync_InputAccepted(StartComputationArguments<int, char> arguments)
	{
		var machine = new Machine<int, char>();

		var raisedStepped = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
			handler => machine.ComputationTerminated += handler,
			handler => machine.ComputationTerminated -= handler,
			() => machine.StartAutomaticAsync(new ComputationRequest<int, char>(arguments.Input, arguments.TransitionTable)));

		Assert.Same(machine, raisedStepped.Sender);
		Assert.Equal(State<int>.Accept, raisedStepped.Arguments.State);
	}

	[Theory]
	[ClassData(typeof(RejectedInputTestData))]
	public async Task StartAutomaticAsync_InputRejected(StartComputationArguments<int, char> arguments)
	{
		var machine = new Machine<int, char>();

		var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
			handler => machine.ComputationTerminated += handler,
			handler => machine.ComputationTerminated -= handler,
			() => machine.StartAutomaticAsync(new ComputationRequest<int, char>(arguments.Input, arguments.TransitionTable)));

		Assert.Same(machine, raisedTerminated.Sender);
		Assert.Equal(State<int>.Reject, raisedTerminated.Arguments.State);
	}

	[Theory]
	[ClassData(typeof(ExpectedTapeOutputTestData))]
	public async Task StartAutomaticAsync_OutputSymbols_SymbolsAsExpected(ExpectedTapeOutputArguments<int, char> arguments)
	{
		var machine = new Machine<int, char>();

		var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
			handler => machine.ComputationTerminated += handler,
			handler => machine.ComputationTerminated -= handler,
			() => machine.StartAutomaticAsync(new ComputationRequest<int, char>(arguments.Input, arguments.TransitionTable)));

		Assert.Same(machine, raisedTerminated.Sender);
		Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
	}
}
