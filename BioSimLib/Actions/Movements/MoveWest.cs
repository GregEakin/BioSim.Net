using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions.Movements;

public class MoveWest : IMovementAction
{
    public Action Type => Action.MOVE_WEST;
    public override string ToString() => "move west";
    public string ShortName => "MvW";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (-actionLevels[(int)Action.MOVE_WEST], 0.0f);
    }
}