namespace BioSimLib.Actions;

public class EmitSignal : IAction
{
    public Action Type => Action.EMIT_SIGNAL0;
    public override string ToString() => "emit signal 0";
    public string ShortName => "SG";
    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
        var emitThreshold = 0.5f;
        var level = actionLevels[(int)Action.EMIT_SIGNAL0];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level *= player.ResponsivenessAdjusted;
        if (level > emitThreshold && Player.Prob2Bool(level)) 
            signals.Increment(0, player._loc);
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}