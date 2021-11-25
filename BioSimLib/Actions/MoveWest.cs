namespace BioSimLib.Actions;

public class MoveWest : IAction
{
    public Action Type => Action.MOVE_WEST;
    public override string ToString() => "move west";
    public string ShortName => "MvW";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (-actionLevels[(int)Action.MOVE_WEST], 0.0f);
    }
}