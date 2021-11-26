using System.ComponentModel.Design;
using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions.Movements;

public class MoveRL : IMovementAction
{
    public Action Type => Action.MOVE_RL;
    public override string ToString() => "move R-L";
    public string ShortName => "MRL";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_RL];
        var offset = lastMoveDir.Rotate90DegCw().AsNormalizedCoord();
        level = 2.0f * (level - 0.5f);
        return (offset.X * level, offset.Y * level);
    }
}