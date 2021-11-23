namespace BioSimLib.Actions;

public class MoveRandom : IAction
{
    public Action Type => Action.MOVE_RANDOM;
    public override string ToString() => "move random";
    public string ShortName => "Mrn";

    public bool IsEnabled() => true;
    public float Calc(Config p, Grid grid, Player player, uint simStep)
    {
        return player._age / p.stepsPerGeneration;
    }
}