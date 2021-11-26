using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using Xunit;

namespace BioSimTests.Positions;

public class GridTests
{
    [Fact]
    public void PopulationDensityAlongAxisTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };
        
        var dir1 = new Dir(Dir.Compass.W);
        var population1 = board.Grid.GetPopulationDensityAlongAxis(loc, dir1);
        Assert.Equal(0.5833333f, population1);

        var dir2 = new Dir(Dir.Compass.N);
        var population2 = board.Grid.GetPopulationDensityAlongAxis(loc, dir2);
        Assert.Equal(0.5f, population2);
    }

    [Fact]
    public void PopulationDensityAlongAxis_CenterTest()
    {
        var p = new Config() { population = 10, populationSensorRadius = 2.0f, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };

        var dir1 = new Dir(Dir.Compass.CENTER);
        var population1 = board.Grid.GetPopulationDensityAlongAxis(loc, dir1);
        Assert.Equal(0.0f, population1);
    }

    [Fact]
    public void VisitNeighborhoodTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        for (var i = 0; i < 5; i++)
            board.NewPlayer(genome, new Coord(1, (short)i));

        var loc = new Coord { X = 2, Y = 2 };

        var squares = 0;
        var critters = 0;
        void F(Coord tloc)
        {
            squares++;
            if (board.Grid.IsOccupiedAt(tloc))
                critters++;
        }

        Grid.VisitNeighborhood(p, loc, 2.0f, F);
        Assert.Equal(13, squares);
        Assert.Equal(3, critters);
    }
}