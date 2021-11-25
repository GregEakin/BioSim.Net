namespace BioSimLib.Actions;

public class MoveX : IAction
{
    public Action Type => Action.MOVE_X;
    public override string ToString() => "move X";
    public string ShortName => "MvX";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}