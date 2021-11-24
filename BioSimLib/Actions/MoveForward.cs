namespace BioSimLib.Actions;

public class MoveForward : IAction
{
    public Action Type => Action.MOVE_FORWARD;
    public override string ToString() => "move fwd";
    public string ShortName => "Mfd";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}