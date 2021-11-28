using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class LastMoveDirYTests
{
    [Fact]
    public void TypeTest()
    {
        var sensor = new LastMoveDirY();
        Assert.Equal(Sensor.LAST_MOVE_DIR_Y, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var sensor = new LastMoveDirY();
        Assert.Equal("last move dir Y", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var sensor = new LastMoveDirY();
        Assert.Equal("LMy", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var genome = new Genome(p, new[] { 0x00000000u });

        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);

        var sensor = new LastMoveDirY();
        Assert.Equal(0.5f, sensor.Output(player, 25));
    }
}