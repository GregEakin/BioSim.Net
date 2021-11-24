namespace BioSimLib.Actions;

public class MoveY : IAction
{
    public Action Type => Action.MOVE_Y;
    public override string ToString() => "move Y";
    public string ShortName => "MvY";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}