namespace BioSimLib.Actions;

public class MoveWest : IAction
{
    public Action Type => Action.MOVE_WEST;
    public override string ToString() => "move west";
    public string ShortName => "MvW";

    public bool IsEnabled() => true;
    public float Calc(Config p, Grid grid, Player player, uint simStep)
    {
        return player._age / p.stepsPerGeneration;
    }
}