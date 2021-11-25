using System.ComponentModel.Design;

namespace BioSimLib.Actions;

public class MoveRL : IAction
{
    public Action Type => Action.MOVE_RL;
    public override string ToString() => "move R-L";
    public string ShortName => "MRL";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RL];
        var offset = level switch
        {
            < 0.0f => lastMoveDir.Rotate90DegCcw().AsNormalizedCoord(),
            > 0.0f => lastMoveDir.Rotate90DegCw().AsNormalizedCoord(),
            _ => new Coord(0, 0)
        };

        return (offset.X * level, offset.Y * level);
    }
}