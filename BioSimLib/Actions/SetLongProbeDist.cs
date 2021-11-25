namespace BioSimLib.Actions;

public class SetLongProbeDist : IAction
{
    public Action Type => Action.SET_LONGPROBE_DIST;
    public override string ToString() => "set longprobe dist";
    public string ShortName => "LPD";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
        var maxLongProbeDistance = 32u;
        var level = actionLevels[(int)Action.SET_LONGPROBE_DIST];
        level = (float)((Math.Tanh(level) + 1.0) / 2.0);
        level = 1.0f + level * maxLongProbeDistance;
        player._longProbeDist = (uint)level;
    }

    public (float, float) Move(float[] actionLevels)
    {
        return (0.0f, 0.0f);
    }
}