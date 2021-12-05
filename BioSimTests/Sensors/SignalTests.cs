using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class SignalTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(p);
        var sensor = new Signal(signals);
        Assert.Equal(Sensor.SIGNAL0, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(p);
        var sensor = new Signal(signals);
        Assert.Equal("signal 0", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config { signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var signals = new Signals(p);
        var sensor = new Signal(signals);
        Assert.Equal("Sg", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config { population = 10, signalLayers = 1, signalSensorRadius = 3, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        var player = board.NewPlayer(genome, new Coord(1, 2));

        var signals = new Signals(p);
        var sensor = new Signal(signals);
        Assert.Equal(0.0f, sensor.Output(player, 0));
    }
}