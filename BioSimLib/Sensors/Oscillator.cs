namespace BioSimLib.Sensors;

public class Oscillator : ISensor
{
    public Sensor Type => Sensor.OSC1;
    public override string ToString() => "oscillator";
    public string ShortName => "Osc";

    public float Output(Player player, uint simStep)
    {
        var phase = simStep % player._oscPeriod / (double)player._oscPeriod;
        var factor = (float)((-Math.Cos(phase * 2.0 * Math.PI) + 1.0) / 2.0);
        var sensorVal = Math.Min(1.0f, Math.Max(0.0f, factor));
        return sensorVal;
    }
}