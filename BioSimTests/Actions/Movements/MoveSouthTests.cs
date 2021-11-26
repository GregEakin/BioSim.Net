using System;
using BioSimLib.Actions.Movements;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions.Movements;

public class MoveSouthTests
{
    [Fact]
    public void MoveDisabledTest()
    {
        var movement = new MoveSouth();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_SOUTH] = 0.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(0.0f, y);
    }

    [Fact]
    public void MoveEnabledTest()
    {
        var movement = new MoveSouth();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_SOUTH] = 1.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(-1.0f, y);
    }
}