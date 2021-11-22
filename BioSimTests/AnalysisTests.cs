using BioSimLib;
using Xunit;

namespace BioSimTests;

public class AnalysisTests
{
    [Fact]
    public void Test1()
    {
        var p = new Params();
        var genome = new Genome(p, new []{ 0x840B7FFFu });
        var net = new NeuralNet(genome);
        Assert.Equal("840B7FFF", net.ToString());
    }
}