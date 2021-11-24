namespace BioSimLib.Actions;

public class Suicide : IAction
{
    public Action Type => Action.SUICIDE;
    public override string ToString() => "suicide";
    public string ShortName => "Die";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
    }
}