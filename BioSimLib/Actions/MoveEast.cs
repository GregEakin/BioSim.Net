namespace BioSimLib.Actions;

public class MoveEast : IAction
{
    public Action Type => Action.MOVE_EAST;
    public override string ToString() => "move east";
    public string ShortName => "MvE";

    public bool Enabled => true;

    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}