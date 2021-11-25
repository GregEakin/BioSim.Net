using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests;

public class PlayerTests
{
    [Fact]
    public void FeedForwardTest()
    {
        var factory = new SensorFactory(
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

        var actionLevels = player.FeedForward(factory, 0);
        Assert.Equal(0.5773243f, player._nnet[0].Output);
        Assert.Equal(0.916998565f, player._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.47961697f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f,
        }, actionLevels);
    }

    [Fact]
    public void MovementTest()
    {
        var p = new Config() { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var peeps = new Peeps(p);
        var grid = new Grid(p, peeps);
        var signals = new Signals(p);
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
        var loc = new Coord { X = 3, Y = 4 };
        var player = grid.NewPlayer(genome, loc);
        player._nnet[0].Driven = true;
        player._nnet[0].Output = 0.6f;
        player._nnet[1].Driven = true;
        player._nnet[1].Output = 0.4f;

        var actionLevels = new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.47961697f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f,
        };

        var factory = new ActionFactory();
        player.ExecuteActions(factory, grid, signals, actionLevels, 0);
    }
}