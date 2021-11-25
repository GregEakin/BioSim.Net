using BioSimLib.Field;
using BioSimLib.Positions;

namespace BioSimLib.Actions;

public class MoveX : IAction
{
    public Action Type => Action.MOVE_X;
    public override string ToString() => "move X";
    public string ShortName => "MvX";

    public bool Enabled => true;
    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (actionLevels[(int)Action.MOVE_X], 0.0f);
    }
}