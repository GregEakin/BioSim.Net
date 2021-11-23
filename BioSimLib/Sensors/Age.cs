namespace BioSimLib.Sensors;

public class Age : ISensor
{
    private readonly Config _p;

    public Age(Config p)
    {
        _p = p;
    }

    public Sensor Type => Sensor.AGE;
    public override string ToString() => "age";
    public string ShortName => "Age";

    public float Output(Player player, uint simStep)
    {
        var sensorVal= (float)(simStep - player._birth) / _p.stepsPerGeneration;
        return sensorVal;
    }
}