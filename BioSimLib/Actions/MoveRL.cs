namespace BioSimLib.Actions;

public class MoveRL : IAction
{
    public Action Type => Action.MOVE_RL;
    public override string ToString() => "move R-L";
    public string ShortName => "MRL";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}