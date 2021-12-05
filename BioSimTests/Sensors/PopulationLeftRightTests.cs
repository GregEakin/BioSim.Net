using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class PopulationLeftRightTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal(Sensor.POPULATION_LR, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal("population LR", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal("Plr", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new GenomeBuilder(p.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);
        player._loc = new Coord(2, 2);

        var sensor = new PopulationLeftRight(board.Grid);
        Assert.Equal(0.5f, sensor.Output(player, 0));
    }
}