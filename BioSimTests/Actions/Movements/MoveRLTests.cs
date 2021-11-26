using System;
using BioSimLib.Actions.Movements;
using BioSimLib.Positions;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Actions.Movements;

public class MoveRLTests
{
    [Fact]
    public void MoveDisabledTest()
    {
        var movement = new MoveRL();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RL] = 0.5f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(0.0f, y);
    }

    [Fact]
    public void MoveLeftTest()
    {
        var movement = new MoveRL();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RL] = 0.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(-1.0f, y);
    }

    [Fact]
    public void MoveRightTest()
    {
        var movement = new MoveRL();
        var actionLevels = new float[Enum.GetNames<Action>().Length];
        actionLevels[(int)Action.MOVE_RL] = 1.0f;
        var lastMoveDir = new Dir(Dir.Compass.W);
        var (x, y) = movement.Move(actionLevels, lastMoveDir);
        Assert.Equal(0.0f, x);
        Assert.Equal(1.0f, y);
    }
}