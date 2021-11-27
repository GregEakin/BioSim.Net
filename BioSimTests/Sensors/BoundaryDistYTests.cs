using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class BoundaryDistYTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDistY(p);
        Assert.Equal(Sensor.BOUNDARY_DIST_Y, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDistY(p);
        Assert.Equal("boundary dist Y", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var sensor = new BoundaryDistY(p);
        Assert.Equal("EDy", sensor.ShortName);
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

        var sensor = new BoundaryDistY(p);
        Assert.Equal(0.8f, sensor.Output(player, 0));
    }
}