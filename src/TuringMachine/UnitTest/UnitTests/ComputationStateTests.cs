using System;
using TuringMachine.Machine;
using Xunit;

namespace TuringMachine.Tests.UnitTests
{
    public class ComputationStateTests
    {
        [Fact]
        public void Constructor_SettingSymbol_Success()
        {
            var symbol = new Symbol<char>('F');
            var computationState = new ComputationState<int, char>(symbol);

            bool isInitialState = computationState.Configuration.State == State<int>.Initial;
            bool isSameSymbol = computationState.Configuration.Symbol == symbol;
            bool isWatchStarted = computationState.Duration > TimeSpan.Zero;

            Assert.True(isInitialState);
            Assert.True(isSameSymbol);
            Assert.False(isWatchStarted);
            Assert.Equal(0, computationState.StepCount);
        }

        [Fact]
        public void UpdateConfiguration_InInitialState_Updated()
        {
            var stateUpdateValue = new State<int>(2);
            var initialSymbol = new Symbol<int>(5);
            var symbolUpdateValue = new Symbol<int>(initialSymbol.Value + 1);
            var computationState = new ComputationState<int, int>(initialSymbol);

            computationState.UpdateConfiguration((stateUpdateValue, symbolUpdateValue));
            bool isStateUpdated = computationState.Configuration.State == stateUpdateValue;
            bool isSymbolUpdated = computationState.Configuration.Symbol == symbolUpdateValue;

            Assert.True(isStateUpdated);
            Assert.True(isSymbolUpdated);
            Assert.Equal(1, computationState.StepCount);
        }
    }
}
