using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class GeneticSimilarityForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal(Sensor.GENETIC_SIM_FWD, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal("genetic similarity forward", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal("Gen", sensor.ShortName);
    }

    [Fact]
    public void Output_EmptyCellTest()
    {
        var p = new Config() { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });

        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal(0.00f, sensor.Output(player, 25));
    }

    [Fact]
    public void Output_DifferentTest()
    {
        var p = new Config() { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome1 = new Genome(p, new[] { 0x00000000u });
        var genome2 = new Genome(p, new[] { 0x00000008u });

        var player = board.NewPlayer(genome1, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);
        board.NewPlayer(genome2, new Coord(1, 2));

        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal(0.00f, sensor.Output(player, 25));
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, genomeComparisonMethod = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome1 = new Genome(p, new[] { 0x00000000u });
        var genome2 = new Genome(p, new[] { 0x00000007u });

        var player = board.NewPlayer(genome1, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);
        board.NewPlayer(genome2, new Coord(1, 2));

        var sensor = new GeneticSimilarityForward(p, board.Grid);
        Assert.Equal(1.00f, sensor.Output(player, 25));
    }
}