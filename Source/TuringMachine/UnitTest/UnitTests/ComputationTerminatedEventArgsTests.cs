using System.Linq;
using TuringMachine.Machine.Computation;
using Xunit;

namespace TuringMachine.Tests.UnitTests;

public class ComputationTerminatedEventArgsTests
{
    private readonly State<int> state;

    public ComputationTerminatedEventArgsTests()
    {
        state = State<int>.Accept;
    }

    [Fact]
    public void TrimResult_EmptyResult_ReturnsEmptyCollection()
    {
        var symbols = Enumerable.Empty<Symbol<char>>();
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, symbols);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Empty(trimmedResult);
    }

    [Fact]
    public void TrimResult_SingleBlankSymbolResult_ReturnsEmptyCollection()
    {
        var symbols = new Symbol<char>[] { Symbol<char>.Blank };
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, symbols);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Empty(trimmedResult);
    }

    [Fact]
    public void TrimResult_OnlyBlankSymbolResult_ReturnsEmptyCollection()
    {
        var symbols = Enumerable.Range(0, 10).Select(i => Symbol<char>.Blank);
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, symbols);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Empty(trimmedResult);
    }

    [Fact]
    public void TrimResult_SingleNonBlankSymbolResult_ReturnsSameSymbols()
    {
        var symbols = "a".Select(c => new Symbol<char>(c));
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, symbols);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Equal(symbols, trimmedResult);
    }

    [Fact]
    public void TrimResult_OnlyNonBlankSymbolResult_ReturnsSameSymbols()
    {
        var symbols = Enumerable.Range(0, 10).Select(i => new Symbol<char>('a'));
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, symbols);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Equal(symbols, trimmedResult);
    }

    [Fact]
    public void TrimResult_StartsWithBlankSymbolsResult_ReturnsBlankSymbolsRemovedFromStart()
    {
        var blankSymbols = Enumerable.Range(0, 10).Select(i => Symbol<char>.Blank);
        var normalSymbols = "aaaaa".Select(c => new Symbol<char>(c));
        var rawResult = blankSymbols.Concat(normalSymbols);
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, rawResult);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Equal(normalSymbols, trimmedResult);
    }

    [Fact]
    public void TrimResult_EndsWithBlankSymbolsResult_ReturnsBlankSymbolsRemovedFromEnd()
    {
        var blankSymbols = Enumerable.Range(0, 10).Select(i => Symbol<char>.Blank);
        var normalSymbols = "aaaaa".Select(c => new Symbol<char>(c));
        var rawResult = normalSymbols.Concat(blankSymbols);
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, rawResult);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Equal(normalSymbols, trimmedResult);
    }

    [Fact]
    public void TrimResult_MiddleContainsBlankSymbolsResult_TrimResult_OnlyNonBlankSymbolResult_ReturnsSameSymbols()
    {
        var blankSymbols = Enumerable.Range(0, 10).Select(i => Symbol<char>.Blank);
        var normalSymbols = "aaaaa".Select(c => new Symbol<char>(c));
        var rawResult = normalSymbols.Concat(blankSymbols).Concat(normalSymbols).Concat(blankSymbols).Concat(normalSymbols);
        var eventArgs = new ComputationTerminatedEventArgs<int, char>(state, rawResult);

        var trimmedResult = eventArgs.TrimResult();

        Assert.Equal(rawResult, trimmedResult);
    }
}
