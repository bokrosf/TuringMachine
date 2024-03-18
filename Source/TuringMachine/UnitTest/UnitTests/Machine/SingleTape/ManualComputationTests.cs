using System;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.SingleTape;
using TuringMachine.Transition.SingleTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape;

public class ManualComputationTests
{
    [Theory]
    [ClassData(typeof(AcceptedInputTestData))]
    public void Step_SteppedRaised(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
        var raisedStepped = Assert.Raises<SteppedEventArgs<Transition<int, char>>>(
            handler => machine.Stepped += handler,
            handler => machine.Stepped -= handler,
            () => machine.Step());

        Assert.Same(machine, raisedStepped.Sender);
    }

    [Theory]
    [ClassData(typeof(AcceptedInputTestData))]
    public void Step_WithoutConstraint_InputAccepted(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
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
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
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
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
        var raisedTerminated = Assert.Raises<ComputationTerminatedEventArgs<int, char>>(
            handler => machine.ComputationTerminated += handler,
            handler => machine.ComputationTerminated -= handler,
            () => StepUntilTermination(machine));

        Assert.Same(machine, raisedTerminated.Sender);
        Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public void Step_ManualAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);

        Assert.Throws<InvalidOperationException>(() => machine.StartManual(arguments.Input));
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public void Abort_NoStepsTaken_Aborted(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
        var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
            handler => machine.ComputationAborted += handler,
            handler => machine.ComputationAborted -= handler,
            () => machine.RequestAbortion());

        Assert.Same(machine, raisedAborted.Sender);
        Assert.Equal(State<int>.Initial, raisedAborted.Arguments.State);
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public void Abort_StepTaken_Aborted(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);
        machine.Step();
        var raisedAborted = Assert.Raises<ComputationAbortedEventArgs<int, char>>(
            handler => machine.ComputationAborted += handler,
            handler => machine.ComputationAborted -= handler,
            () => machine.RequestAbortion());

        Assert.Same(machine, raisedAborted.Sender);
        Assert.NotEqual(State<int>.Initial, raisedAborted.Arguments.State);
        Assert.False(raisedAborted.Arguments.State.IsFinishState);
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public async Task Abort_AutomaticAlreadyStarted_ThrowsExceptionAsync(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);
        Task firstStepSynchronizationTask = new Task(() => { });
        Task abortionSynchronizationTask = new Task(() => { });
        bool hasRaisedAborted = false;

        void Machine_Stepped(object? sender, SteppedEventArgs<Transition<int, char>> e)
        {
            machine.Stepped -= Machine_Stepped;
            firstStepSynchronizationTask.Start();
        }

        void Machine_ComputationAborted(object? sender, ComputationAbortedEventArgs<int, char> e)
        {
            machine.Stepped -= Machine_Stepped;
            hasRaisedAborted = true;
            abortionSynchronizationTask.Start();
        }

        machine.Stepped += Machine_Stepped;
        machine.ComputationAborted += Machine_ComputationAborted;

        // Thread creation needed because using Task.Run() can cause the infinite computation run forever because scheduling.
        Thread computationThread = new Thread(async () => await machine.StartAutomaticComputationAsync(arguments.Input));
        computationThread.Priority = ThreadPriority.Lowest;
        computationThread.Start();
        await firstStepSynchronizationTask;
        machine.RequestAbortion();
        await abortionSynchronizationTask;
        computationThread.Join();

        Assert.True(hasRaisedAborted);
    }


    private void StepUntilTermination<TState, TSymbol>(Machine<TState, TSymbol> machine)
    {
        while (machine.Step())
        {
            ;
        }
    }
}
