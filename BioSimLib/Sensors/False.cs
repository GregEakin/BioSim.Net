namespace BioSimLib.Sensors;

public class False : ISensor
{
    public Sensor Type => Sensor.FALSE;
    public override string ToString() => "false";
    public string ShortName => "F";

    public float Output(Player player, uint simStep)
    {
        return 0.0f;
    }
}