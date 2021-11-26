using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class PopulationForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationForward(board.Grid);

        Assert.Equal(Sensor.POPULATION_FWD, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationForward(board.Grid);

        Assert.Equal("population forward", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationForward(board.Grid);

        Assert.Equal("Pfd", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);
        player._loc = new Coord(2, 2);

        var sensor = new PopulationForward(board.Grid);
        Assert.Equal(0.5833333f, sensor.Output(player, 0));
    }
}