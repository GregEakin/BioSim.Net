namespace BioSimLib.Sensors;

public class Age : ISensor
{
    public Sensor Type => Sensor.AGE;
    public override string ToString() => "age";
    public string ShortName => "Age";

    public float Output(Params p, Indiv indiv, uint simStep)
    {
        return indiv._age / p.stepsPerGeneration;
    }
}