namespace BioSimLib.Sensors;

public class Signal : ISensor
{
    private readonly Signals _signals;

    public Signal(Signals signals)
    {
        _signals = signals;
    }

    public Sensor Type => Sensor.SIGNAL0;
    public override string ToString() => "signal 0";
    public string ShortName => "Sg";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _signals.GetSignalDensity(0u, player._loc);
        return sensorVal;
    }
}