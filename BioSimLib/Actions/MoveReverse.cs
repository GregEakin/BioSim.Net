namespace BioSimLib.Actions;

public class MoveReverse : IAction
{
    public Action Type => Action.MOVE_REVERSE;
    public override string ToString() => "move reverse";
    public string ShortName => "Mrv";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}