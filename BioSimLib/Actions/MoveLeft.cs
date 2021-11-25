namespace BioSimLib.Actions;

public class MoveLeft : IAction
{
    public Action Type => Action.MOVE_LEFT;
    public override string ToString() => "move left";
    public string ShortName => "MvL";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}