using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class BoundaryDistTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new BoundaryDist(p);
        Assert.Equal(Sensor.BOUNDARY_DIST, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new BoundaryDist(p);
        Assert.Equal("boundary dist", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new BoundaryDist(p);
        Assert.Equal("ED", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, shortProbeBarrierDistance = 2, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });
        board.NewBarrier(new Coord(2, 0));

        var player = board.NewPlayer(genome, new Coord(1, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new BoundaryDist(p);
        Assert.Equal(1.0f, sensor.Output(player, 0));
    }
}