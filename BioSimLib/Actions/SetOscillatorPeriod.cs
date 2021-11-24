namespace BioSimLib.Actions;

public class SetOscillatorPeriod : IAction
{
    public Action Type => Action.SET_OSCILLATOR_PERIOD;
    public override string ToString() => "set osc1";
    public string ShortName => "Osc";

    public bool Enabled => true;
    public void Execute(Config p, Grid grid, Signals signals, Player player, uint simStep, float[] actionLevels)
    {
        var period = actionLevels[(int)Action.SET_OSCILLATOR_PERIOD];
        var newPeriodF01 = (float)((Math.Tanh(period) + 1.0f) / 2.0f); 
        var newPeriod = 1u + (uint)(1.5f + Math.Exp(7.0f * newPeriodF01));
        player._oscPeriod = newPeriod;
    }
}