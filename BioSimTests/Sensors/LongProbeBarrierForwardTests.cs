using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests.Sensors;

public class LongProbeBarrierForwardTests
{
    [Fact]
    public void TypeTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal(Sensor.LONGPROBE_BAR_FWD, sensor.Type);
    }

    [Fact]
    public void StringTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal("long probe barrier fwd", sensor.ToString());
    }

    [Fact]
    public void ShortNameTest()
    {
        var p = new Config() { sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal("LPb", sensor.ShortName);
    }

    [Fact]
    public void OutputTest()
    {
        var p = new Config() { population = 10, sizeX = 5, sizeY = 5 };
        var board = new Board(p);
        board.NewBarrier(new Coord(1, 2));

        var genome = new Genome(p, new[] { 0x00000000u });
        var player = board.NewPlayer(genome, new Coord(2, 2));
        player.LastMoveDir = new Dir(Dir.Compass.W);
        player._longProbeDist = 2;

        var sensor = new LongProbeBarrierForward(board.Grid);
        Assert.Equal(0.5f, sensor.Output(player, 25));
    }
}