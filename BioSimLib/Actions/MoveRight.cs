using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class MoveRight : IAction
{
    public Action Type => Action.MOVE_RIGHT;
    public override string ToString() => "move right";
    public string ShortName => "MvR";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RIGHT];
        var offset = lastMoveDir.Rotate90DegCw().AsNormalizedCoord();
        return (offset.X * level, offset.Y * level);
    }
}