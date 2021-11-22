using BioSimLib;
using Xunit;

namespace BioSimTests;

public class AnalysisTests
{
    [Fact]
    public void Test1()
    {
        var p = new Params();
        var genom = new Genome(p);
        var net = new NeuralNet(genom);
        Assert.Equal("", net.ToString());
    }
}