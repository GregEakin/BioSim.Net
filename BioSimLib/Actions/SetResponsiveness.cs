namespace BioSimLib.Actions;

public class SetResponsiveness : IAction
{
    public Action Type => Action.SET_RESPONSIVENESS;
    public override string ToString() => "set inv-responsiveness";
    public string ShortName => "Res";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
        var level = actionLevels[(int)Action.SET_RESPONSIVENESS];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0); // convert to 0.0..1.0
        player._responsiveness = level;
    }

    public (float, float) Move(float[] actionLevels, Dir lastMoveDir)
    {
        return (0.0f, 0.0f);
    }
}