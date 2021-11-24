namespace BioSimLib.Sensors;

public class True : ISensor
{
    public Sensor Type => Sensor.TRUE;
    public override string ToString() => "true";
    public string ShortName => "T";

    public float Output(Player player, uint simStep)
    {
        return 1.0f;
    }
}