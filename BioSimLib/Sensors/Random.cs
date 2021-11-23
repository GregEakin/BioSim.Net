namespace BioSimLib.Sensors;

public class Random : ISensor
{
    public Sensor Type => Sensor.RANDOM;
    public override string ToString() => "random";
    public string ShortName => "Rnd";

    public float Output(Config p, Player player, uint simStep)
    {
        return player._age / p.stepsPerGeneration;
    }
}