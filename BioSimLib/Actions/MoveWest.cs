namespace BioSimLib.Actions;

public class MoveWest : IAction
{
    public Action Type => Action.MOVE_WEST;
    public override string ToString() => "move west";
    public string ShortName => "MvW";

    public bool IsEnabled() => true;
    public float Calc(Params p, Grid grid, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}