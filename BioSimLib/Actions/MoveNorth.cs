using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class MoveNorth : IAction
{
    public Action Type => Action.MOVE_NORTH;
    public override string ToString() => "move north";
    public string ShortName => "MvN";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, actionLevels[(int)Action.MOVE_NORTH]);
    }
}