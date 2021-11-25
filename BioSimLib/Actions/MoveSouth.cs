namespace BioSimLib.Actions;

public class MoveSouth : IAction
{
    public Action Type => Action.MOVE_SOUTH;
    public override string ToString() => "move south";
    public string ShortName => "MvS";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}