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
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal(Sensor.POPULATION_LR, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal("population LR", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var config = new Config { sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var sensor = new PopulationLeftRight(board.Grid);

        Assert.Equal("Plr", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var config = new Config { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(config);
        var genome = new GenomeBuilder(config.maxNumberNeurons, new[] { 0x00000000u }).ToGenome();
        for (var i = 0; i < 5; i++)
            board.NewCritter(genome, new Coord(1, (short)i));

        var critter = board.NewCritter(genome, new Coord(2, 2));
        critter.LastMoveDir = new Dir(Dir.Compass.W);
        critter.Loc = new Coord(2, 2);

        var sensor = new PopulationLeftRight(board.Grid);
        Assert.Equal(0.5f, sensor.Output(critter, 0));
    }
}