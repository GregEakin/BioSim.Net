using BioSimLib.Field;

namespace BioSimLib.Sensors;

public class SignalLR : ISensor
{
    private readonly Signals _signals;

    public SignalLR(Signals signals)
    {
        _signals = signals;
    }

    public Sensor Type => Sensor.SIGNAL0_LR;
    public override string ToString() => "signal 0 LR";
    public string ShortName => "Slr";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _signals.GetSignalDensityAlongAxis(0u, player._loc, player.LastMoveDir.Rotate90DegCw());
        return sensorVal;
    }
}