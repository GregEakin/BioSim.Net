namespace BioSimLib.Actions;

public class MoveRandom : IAction
{
    public Action Type => Action.MOVE_RANDOM;
    public override string ToString() => "move random";
    public string ShortName => "Mrn";

    public bool IsEnabled() => true;
    public float Calc(Params p, Grid grid, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}