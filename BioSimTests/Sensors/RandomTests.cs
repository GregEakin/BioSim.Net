using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;
using Random = BioSimLib.Sensors.Random;

namespace BioSimTests.Sensors;

public class RandomTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new Random();
        Assert.Equal(Sensor.RANDOM, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new Random();
        Assert.Equal("random", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new Random();
        Assert.Equal("Rnd", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        var player = board.NewCritter(genome, new Coord(1, 2));

        var sensor = new Random();
        // Assert.Equal(1.0f, sensor.Output(player, 0));
    }
}