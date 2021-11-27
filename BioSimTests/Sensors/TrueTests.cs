using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class TrueTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new True();
        Assert.Equal(Sensor.TRUE, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new True();
        Assert.Equal("true", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new True();
        Assert.Equal("T", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        var player = board.NewPlayer(genome, new Coord(1, 2));

        var sensor = new True();
        Assert.Equal(1.0f, sensor.Output(player, 0));
    }
}