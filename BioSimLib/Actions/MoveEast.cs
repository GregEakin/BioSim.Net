namespace BioSimLib.Actions;

public class MoveEast : IAction
{
    public Action Type => Action.MOVE_EAST;
    public override string ToString() => "move east";
    public string ShortName => "MvE";

    public bool Enabled => true;

    public void Execute(Config p, Board board, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (actionLevels[(int)Action.MOVE_EAST], 0.0f);
    }
}