namespace BioSimLib.Actions;

public class MoveRight : IAction
{
    public Action Type => Action.MOVE_RIGHT;
    public override string ToString() => "move right";
    public string ShortName => "MvR";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}