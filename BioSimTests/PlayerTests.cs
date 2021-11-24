using BioSimLib;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests;

public class PlayerTests
{
    [Fact]
    public void Test1()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.8f),
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
            }
        );

        var p = new Config() { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var peeps = new Peeps(p);
        var grid = new Grid(p, peeps);
        var dna = new[]
        {
            0x01012000u,
            0x00012000u,
            0x80002000u,
            0x01002000u,
            0x018A2000u,
            0x91842000u,
            0x01842000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var player = grid.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.6f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.4f;

        var actionLevels = player.FeedForward(sensors, 0);
        Assert.Equal(0.7615942f, player._nnet[0].Output);
        Assert.Equal(0.88535166f, player._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.8615942f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.7615942f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }

    [Fact]
    public void Test2()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
            }
        );

        var p = new Config() { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var peeps = new Peeps(p);
        var grid = new Grid(p, peeps);
        var dna = new[]
        {
            0x018A2000u,
            0x91842000u,
            0x01842000u,
            0x01012000u,
            0x00012000u,
            0x80002000u,
            0x01002000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var player = grid.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.4f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.6f;

        var actionLevels = player.FeedForward2(sensors, 0);
        Assert.Equal(0.7615942f, player._nnet[0].Output);
        Assert.Equal(0.5370496f, player._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.462117165f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }

    [Fact]
    public void Test3()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
            }
        );

        var p = new Config() { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var peeps = new Peeps(p);
        var grid = new Grid(p, peeps);
        var dna = new[]
        {
            0x00012000u,
            0x01002000u,
            0x018A2000u,
            0x01842000u,
            0x01012000u,
            0x80002000u,
            0x91842000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var player = grid.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.6f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.4f;

        var actionLevels = player.FeedForward4(sensors, 0);
        Assert.Equal(0.5773243f, player._nnet[0].Output);
        Assert.Equal(0.916998565f, player._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.47961697f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }
}