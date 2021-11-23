namespace BioSimLib.Sensors;

public class Random : ISensor
{
    private readonly System.Random _random = new();

    public Sensor Type => Sensor.RANDOM;
    public override string ToString() => "random";
    public string ShortName => "Rnd";

    public float Output(Player player, uint simStep)
    {
        var sensorVal = _random.NextSingle();
        return sensorVal;
    }
}