using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class LastMoveDirXTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal(Sensor.LAST_MOVE_DIR_X, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal("last move dir X", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new LastMoveDirX();
        Assert.Equal("LMx", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });

        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new LastMoveDirX();
        Assert.Equal(0.0f, sensor.Output(player, 25));
    }
}