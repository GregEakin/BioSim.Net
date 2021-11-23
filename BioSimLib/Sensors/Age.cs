namespace BioSimLib.Sensors;

public class Age : ISensor
{
    public Sensor Type => Sensor.AGE;
    public override string ToString() => "age";
    public string ShortName => "Age";

    public float Output(Params p, Player player, uint simStep)
    {
        return player._age / p.stepsPerGeneration;
    }
}