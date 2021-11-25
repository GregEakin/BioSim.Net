using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class MoveReverse : IAction
{
    public Action Type => Action.MOVE_REVERSE;
    public override string ToString() => "move reverse";
    public string ShortName => "Mrv";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_REVERSE];
        var offset = lastMoveDir.AsNormalizedCoord();
        return (-offset.X * level, -offset.Y * level);
    }
}