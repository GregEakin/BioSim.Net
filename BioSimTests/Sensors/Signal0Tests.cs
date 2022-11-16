using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class Signal0Tests
{
    [Fact]
    public void TypeTest()
    {
        var config = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(config);
        var sensor = new Signal0(signals);
        Assert.Equal(Sensor.SIGNAL0, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var config = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(config);
        var sensor = new Signal0(signals);
        Assert.Equal("signal 0", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var config = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(config);
        var sensor = new Signal0(signals);
        Assert.Equal("Sg", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, signalLayers = 1, signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        var critter = board.NewCritter(genome, new Coord(1, 2));

        var signals = new Signals(config);
        var sensor = new Signal0(signals);
        Assert.Equal(0.0f, sensor.Output(critter, 0));
    }
}