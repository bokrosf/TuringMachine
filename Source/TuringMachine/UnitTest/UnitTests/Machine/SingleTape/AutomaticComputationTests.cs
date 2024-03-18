using System;
using System.Threading;
using System.Threading.Tasks;
using TuringMachine.Machine.Computation;
using TuringMachine.Machine.SingleTape;
using TuringMachine.Transition.SingleTape;
using Xunit;

namespace TuringMachine.Tests.UnitTests.Machine.SingleTape;

public class AutomaticComputationTests
{
    [Theory]
    [ClassData(typeof(AcceptedInputTestData))]
    public async Task StartAutomaticAsync_SteppedRaised(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        var raisedStepped = await Assert.RaisesAsync<SteppedEventArgs<Transition<int, char>>>(
            handler => machine.Stepped += handler,
            handler => machine.Stepped -= handler,
            () => machine.StartAutomaticAsync(arguments.Input));

        Assert.Same(machine, raisedStepped.Sender);
        Assert.Equal(State<int>.Accept, raisedStepped.Arguments.Transition.Range.State);
    }

    [Theory]
    [ClassData(typeof(AcceptedInputTestData))]
    public async Task StartAutomaticAsync_InputAccepted(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
            handler => machine.ComputationTerminated += handler,
            handler => machine.ComputationTerminated -= handler,
            () => machine.StartAutomaticAsync(arguments.Input));

        Assert.Same(machine, raisedTerminated.Sender);
        Assert.Equal(State<int>.Accept, raisedTerminated.Arguments.State);
    }

    [Theory]
    [ClassData(typeof(RejectedInputTestData))]
    public async Task StartAutomaticAsync_InputRejected(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
            handler => machine.ComputationTerminated += handler,
            handler => machine.ComputationTerminated -= handler,
            () => machine.StartAutomaticAsync(arguments.Input));

        Assert.Same(machine, raisedTerminated.Sender);
        Assert.Equal(State<int>.Reject, raisedTerminated.Arguments.State);
    }

    [Theory]
    [ClassData(typeof(ExpectedTapeOutputTestData))]
    public async Task StartAutomaticAsync_OutputSymbols_SymbolsAsExpected(ExpectedTapeOutputArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        var raisedTerminated = await Assert.RaisesAsync<ComputationTerminatedEventArgs<int, char>>(
            handler => machine.ComputationTerminated += handler,
            handler => machine.ComputationTerminated -= handler,
            () => machine.StartAutomaticAsync(arguments.Input));

        Assert.Same(machine, raisedTerminated.Sender);
        Assert.Equal(arguments.ExpectedOutput, raisedTerminated.Arguments.TrimResult());
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public async Task StartAutomaticAsync_AutomaticAlreadyStarted_ThrowsException(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);
        Task firstStepSynchronizationTask = new Task(() => { });

        void HandleMachineStepped(object? sender, SteppedEventArgs<Transition<int, char>> e)
        {
            machine.Stepped -= HandleMachineStepped;
            firstStepSynchronizationTask.Start();
        }

        machine.Stepped += HandleMachineStepped;

        // Thread creation needed because using Task.Run() can cause the infinite computation run forever because scheduling.
        Thread computationThread = new Thread(async () => await machine.StartAutomaticAsync(arguments.Input));
        computationThread.Priority = ThreadPriority.Lowest;
        computationThread.Start();
        await firstStepSynchronizationTask;
        await Assert.ThrowsAsync<InvalidOperationException>(() => machine.StartAutomaticAsync(arguments.Input));
        machine.RequestAbortion();
        computationThread.Join();
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public async Task StartAutomaticAsync_CancellationToken_Aborted(StartComputationArguments<int, char> arguments)
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
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        Thread computationThread = new Thread(async () => await machine.StartAutomaticAsync(arguments.Input, cancellationToken));
        computationThread.Priority = ThreadPriority.Lowest;
        computationThread.Start();
        await firstStepSynchronizationTask;
        cancellationTokenSource.Cancel();
        await abortionSynchronizationTask;
        computationThread.Join();

        Assert.True(hasRaisedAborted);
    }

    [Theory]
    [ClassData(typeof(InfiniteComputationTestData))]
    public async Task StartAutomaticAsync_ManualAlreadyStarted_ThrowsException(StartComputationArguments<int, char> arguments)
    {
        var machine = new Machine<int, char>(arguments.TransitionTable);

        machine.StartManual(arguments.Input);

        await Assert.ThrowsAsync<InvalidOperationException>(() => machine.StartAutomaticAsync(arguments.Input));
    }
}
