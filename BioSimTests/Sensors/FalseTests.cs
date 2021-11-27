using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class FalseTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new False();
        Assert.Equal(Sensor.FALSE, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new False();
        Assert.Equal("false", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new False();
        Assert.Equal("F", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        var player = board.NewPlayer(genome, new Coord(1, 2));

        var sensor = new False();
        Assert.Equal(0.0f, sensor.Output(player, 0));
    }
}