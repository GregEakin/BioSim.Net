namespace BioSimLib.Sensors;

public class SignalFwd : ISensor
{
    private readonly Signals _signals;

    public SignalFwd(Signals signals)
    {
        _signals = signals;
    }

    public Sensor Type => Sensor.SIGNAL0_FWD;
    public override string ToString() => "signal 0";
    public string ShortName => "Sfd";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _signals.GetSignalDensityAlongAxis(0u, player._loc, player._lastMoveDir);
        return sensorVal;
    }
}