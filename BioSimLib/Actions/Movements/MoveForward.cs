using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions.Movements;

public class MoveForward : IMovementAction
{
    public Action Type => Action.MOVE_FORWARD;
    public override string ToString() => "move fwd";
    public string ShortName => "Mfd";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        var level = actionLevels[(int)Action.MOVE_FORWARD];
        var offset = lastMoveDir.AsNormalizedCoord();
        return (offset.X * level, offset.Y * level);
    }
}