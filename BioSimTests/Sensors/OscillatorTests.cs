using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class OscillatorTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new Oscillator();
        Assert.Equal(Sensor.OSC1, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new Oscillator();
        Assert.Equal("oscillator", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new Oscillator();
        Assert.Equal("Osc", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        var critter = board.NewCritter(genome, new Coord(1, 2));

        var sensor = new Oscillator();
        Assert.Equal(0.0f, sensor.Output(critter, 0));
    }
}